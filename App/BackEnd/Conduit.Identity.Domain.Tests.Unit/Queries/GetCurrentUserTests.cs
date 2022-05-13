using System.Collections.Generic;
using System.Threading.Tasks;
using Conduit.Core.Context;
using Conduit.Core.Validation;
using Conduit.Identity.Domain.Configuration;
using Conduit.Identity.Domain.Contracts.Commands.LoginUser;
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
        private readonly User _user;
        private readonly IMediator _mediator;

        public GetCurrentUserTests()
        {
            _user = new User
            {
                Id = 1,
                Email = "solo@yolo.com",
                Username = "soloyolo",
                Password = PasswordHasher.HashPassword(null, PlainTextPassword)
            };
            
            var userRepo = new Mock<IUserRepository>();
            userRepo.Setup(repository => repository.Exists(It.Is<int>(id => id == _user.Id))).Returns(Task.FromResult(true));
            userRepo.Setup(repository => repository.GetById(It.Is<int>(id => id == _user.Id))).Returns(Task.FromResult(_user));
            userRepo.Setup(repository => repository.ExistsByEmail(It.Is<string>(email => email.Equals(_user.Email)))).Returns(Task.FromResult(true));
            userRepo.Setup(repository => repository.GetByEmail(It.Is<string>(email => email.Equals(_user.Email)))).Returns(Task.FromResult(_user));
            
            var identityModule = new IdentityModule();
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder();
            configuration.AddInMemoryCollection(new Dictionary<string, string>
            {
                {$"{nameof(JwtSettings)}:{nameof(JwtSettings.Secret)}", "secretsecretsecretsecretsecretsecret"},
                {$"{nameof(JwtSettings)}:{nameof(JwtSettings.ValidIssuer)}", "issuer"},
                {$"{nameof(JwtSettings)}:{nameof(JwtSettings.ValidAudience)}", "audience"},
            });
            
            services.AddSingleton(typeof(ILogger<>), typeof(NullLogger<>));
            identityModule.AddServices(configuration.Build(), services);
            identityModule.ReplaceSingleton(userRepo.Object);
            identityModule.ReplaceSingleton<IPasswordHasher<User>, BCryptPasswordHasher<User>>(PasswordHasher);
            identityModule.ReplaceScoped<IUserContext>(new TestUserContext(
                _user.Id, 
                _user.Username,
                _user.Email,
                Token));
            
            var provider = services.BuildServiceProvider();
            _mediator = provider.GetRequiredService<IMediator>();
        }
        
        [Fact]
        public async Task GivenAuthenticatedUser_WhenGetCurrentUser_ThenUserIsReturned()
        {
            //arrange
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
        public async Task GivenNonExistentUser_WhenGetCurrentUser_ThenFailsValidation()
        {
            //arrange
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
    }
}