using System;
using System.Threading.Tasks;
using Conduit.Core.Validation;
using Conduit.Identity.Domain.Contracts.LoginUser;
using Conduit.Identity.Domain.Contracts.RegisterUser;
using Conduit.Identity.Domain.Entities;
using Conduit.Identity.Domain.Interactions.Outbound.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using ScottBrady91.AspNetCore.Identity;
using Xunit;

namespace Conduit.Identity.Domain.Tests.Unit
{
    public class LoginUserTests
    {
        private const string PlainTextPassword = "soloyolo";
        private static readonly BCryptPasswordHasher<User> PasswordHasher = new ();
        private readonly User _user;
        private readonly IMediator _mediator;

        public LoginUserTests()
        {
            _user = new User
            {
                Id = 1,
                Email = "solo@yolo.com",
                Username = "soloyolo",
                Password = PasswordHasher.HashPassword(null, PlainTextPassword)
            };
            
            var userRepo = new Mock<IUserRepository>();
            userRepo.Setup(repository => repository.ExistsByEmail(It.Is<string>(s => s.Equals(_user.Email)))).Returns(Task.FromResult(true));
            userRepo.Setup(repository => repository.GetByEmail(It.Is<string>(s => s.Equals(_user.Email)))).Returns(Task.FromResult(_user));
            
            var services = new ServiceCollection();
            services.AddTransient(_ => userRepo.Object);
            services.AddSingleton(typeof(ILogger<>), typeof(NullLogger<>));
            services.AddSingleton<IPasswordHasher<User>, BCryptPasswordHasher<User>>(_ => PasswordHasher);
            
            services.AddMediatR(IdentityDomain.Assembly);
            services.AddValidatorsFromAssembly(IdentityDomain.Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CommandValidationPipelineBehavior<,>));

            var provider = services.BuildServiceProvider();
            _mediator = provider.GetRequiredService<IMediator>();
        }
        
        [Fact]
        public async Task GivenValidCredentials_WhenLoginUser_ThenUserIsAuthenticated()
        {
            //arrange
            var loginUserCommand = new LoginUserCommand
            {
                UserCredentials = new UserCredentialsDTO
                {
                    Email = _user.Email,
                    Password = PlainTextPassword
                }
            };

            //act
            var result = await _mediator.Send(loginUserCommand);
            
            //assert
            Assert.True(result.Result == OperationResult.Success);
            Assert.True(result.Response.IsAuthenticated);
            var loggedInUser = result.Response.LoggedInUser;
            Assert.NotNull(loggedInUser);
            Assert.Equal(_user.Email, loggedInUser.Email);
            Assert.Equal(_user.Username, loggedInUser.Username);
            Assert.NotEmpty(loggedInUser.Token);
        }
        
        [Fact]
        public async Task GivenIncorrectPassword_WhenLoginUser_ThenUserIsNotAuthenticated()
        {
            //arrange
            var loginUserCommand = new LoginUserCommand
            {
                UserCredentials = new UserCredentialsDTO
                {
                    Email = _user.Email,
                    Password = "incorrectPassword" 
                }
            };

            //act
            var result = await _mediator.Send(loginUserCommand);
            
            //assert
            Assert.True(result.Result == OperationResult.Success);
            Assert.False(result.Response.IsAuthenticated);
            var loggedInUser = result.Response.LoggedInUser;
            Assert.Null(loggedInUser);
        }
        
        [Fact]
        public async Task GivenEmailNotRegistered_WhenLoginUser_ThenValidationFails()
        {
            //arrange
            var loginUserCommand = new LoginUserCommand
            {
                UserCredentials = new UserCredentialsDTO
                {
                    Email = "hello@example.com",
                    Password = PlainTextPassword
                }
            };

            //act
            var result = await _mediator.Send(loginUserCommand);
            
            //assert
            Assert.True(result.Result == OperationResult.ValidationError);
            Assert.Null(result.Response);
        }
    }
}