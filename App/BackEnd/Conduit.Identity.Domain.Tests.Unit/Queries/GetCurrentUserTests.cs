using System.Collections.Generic;
using System.Threading.Tasks;
using Conduit.Core.Context;
using Conduit.Core.PipelineBehaviors;
using Conduit.Identity.Domain.Configuration;
using Conduit.Identity.Domain.Contracts.Queries.GetCurrentUser;
using Conduit.Identity.Domain.Entities;
using Conduit.Identity.Domain.Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using ScottBrady91.AspNetCore.Identity;
using Xunit;

namespace Conduit.Identity.Domain.Tests.Unit.Queries
{
    public class GetCurrentUserTests
    {
        private const string Token = "jwt";
        private const string PlainTextPassword = "soloyolo";
        private static readonly BCryptPasswordHasher<User> PasswordHasher = new ();
        private User _user;
        private IMediator _mediator;
        private readonly IdentityModule _identityModule;
        private readonly IServiceCollection _services;
        private readonly ConfigurationBuilder _configuration;
        private IUserContext _userContext;
        private readonly Mock<IUserRepository> _userRepo;

        public GetCurrentUserTests()
        {
            _user = new User
            {
                Id = 1,
                Email = "solo@yolo.com",
                Username = "soloyolo",
                Password = PasswordHasher.HashPassword(null, PlainTextPassword)
            };
            
            _userRepo = new Mock<IUserRepository>();
            _userRepo.Setup(repository => repository.Exists(It.Is<int>(id => id == _user.Id))).Returns(Task.FromResult(true));
            _userRepo.Setup(repository => repository.GetById(It.Is<int>(id => id == _user.Id))).Returns(Task.FromResult(_user));
            _userRepo.Setup(repository => repository.ExistsByEmail(It.Is<string>(email => email.Equals(_user.Email)))).Returns(Task.FromResult(true));
            _userRepo.Setup(repository => repository.GetByEmail(It.Is<string>(email => email.Equals(_user.Email)))).Returns(Task.FromResult(_user));
            
            _identityModule = new IdentityModule();
            _services = new ServiceCollection();
            _configuration = new ConfigurationBuilder();
            _configuration.AddInMemoryCollection(new Dictionary<string, string>
            {
                {$"{nameof(JwtSettings)}:{nameof(JwtSettings.Secret)}", "secretsecretsecretsecretsecretsecret"},
                {$"{nameof(JwtSettings)}:{nameof(JwtSettings.ValidIssuer)}", "issuer"},
                {$"{nameof(JwtSettings)}:{nameof(JwtSettings.ValidAudience)}", "audience"},
            });

            _userContext = new TestUserContext(_user.Id, _user.Username, _user.Email, Token);
        }

        private void BuildDIContainer()
        {
            _services.AddSingleton(typeof(ILogger<>), typeof(NullLogger<>));
            _identityModule.AddServices(_configuration.Build(), _services);
            _identityModule.ReplaceSingleton(_userRepo.Object);
            _identityModule.ReplaceSingleton<IPasswordHasher<User>, BCryptPasswordHasher<User>>(PasswordHasher);
            _identityModule.ReplaceScoped<IUserContext>(_userContext);
            
            var provider = _services.BuildServiceProvider();
            _mediator = provider.GetRequiredService<IMediator>();
        }
        
        [Fact]
        public async Task GivenAuthenticatedUser_WhenGetCurrentUser_ThenUserIsReturned()
        {
            //arrange
            BuildDIContainer();
            var getCurrentUserQuery = new GetCurrentUserQuery();

            //act
            var result = await _mediator.Send(getCurrentUserQuery);
            
            //assert
            Assert.True(result.Result == OperationResult.Success);
            var currentUser = result.Response.CurrentUser;
            Assert.NotNull(currentUser);
            Assert.Equal(_user.Email, currentUser.Email);
            Assert.Equal(_user.Username, currentUser.Username);
            Assert.NotEmpty(currentUser.Token);
        }
        
        [Fact]
        public async Task GivenUnauthenticatedUser_WhenGetCurrentUser_ThenNotAuthenticated()
        {
            //arrange
            _userContext = new TestUserContext();
            BuildDIContainer();
            var getCurrentUserQuery = new GetCurrentUserQuery();

            //act
            var result = await _mediator.Send(getCurrentUserQuery);
            
            //assert
            Assert.True(result.Result == OperationResult.NotAuthenticated);
            Assert.Null(result.Response);
        }
    }
}