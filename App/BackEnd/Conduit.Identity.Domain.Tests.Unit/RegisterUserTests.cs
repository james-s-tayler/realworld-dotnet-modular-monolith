using System;
using System.Threading.Tasks;
using Conduit.Core.Validation;
using Conduit.Identity.Domain.Contracts.RegisterUser;
using Conduit.Identity.Domain.Entities;
using Conduit.Identity.Domain.Interactions.Outbound.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace Conduit.Identity.Domain.Tests.Unit
{
    public class RegisterUserTests
    {
        //should refactor these tests to make the arrange phase less noisy - use either a fixture or helper methods

        [Fact]
        public async Task GivenANewUser_WhenRegistered_ThenNewUserIdReturned()
        {
            //arrange
            var random = new Random();
            var userId = random.Next(1, 1000);

            User registeredUser = null; 
            
            var userRepo = new Mock<IUserRepository>();
            userRepo.Setup(repository => repository.Create(It.IsAny<User>()))
                .Callback<User>(u => registeredUser = u)
                .Returns(Task.FromResult(userId));
            

            var services = new ServiceCollection();
            services.AddTransient(_ => userRepo.Object);
            services.AddSingleton(typeof(ILogger<>), typeof(NullLogger<>));
            
            services.AddMediatR(IdentityDomain.Assembly);
            services.AddValidatorsFromAssembly(IdentityDomain.Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CommandValidationPipelineBehavior<,>));

            var provider = services.BuildServiceProvider();
            var mediatR = provider.GetRequiredService<IMediator>();

            var registerUserCommand = new RegisterUserCommand
            {
                NewUser = new NewUserDTO
                {
                    Email = "solo@yolo.com",
                    Username = "soloyolo",
                    Password = "soloyolo"
                }
            };

            //act
            var result = await mediatR.Send(registerUserCommand);
            
            //assert
            Assert.True(result.IsValidResponse);
            Assert.Equal(userId, result.Result.UserId);
            Assert.NotNull(registeredUser);
            Assert.Equal(registerUserCommand.NewUser.Email, registeredUser.Email);
            Assert.Equal(registerUserCommand.NewUser.Username, registeredUser.Username);
            Assert.Equal(registerUserCommand.NewUser.Password, registeredUser.Password);
        }
        
        [Fact]
        public async Task GivenAUsernameAlreadyInUse_WhenRegistered_ThenFailsValidation()
        {
            //arrange
            var random = new Random();
            var userId = random.Next(1, 1000);
            
            var userRepo = new Mock<IUserRepository>();
            userRepo.Setup(repository => repository.Create(It.IsAny<User>())).Returns(Task.FromResult(userId));
            userRepo.Setup(repository => repository.ExistsByEmail(It.IsAny<string>())).Returns(Task.FromResult(false));
            userRepo.Setup(repository => repository.ExistsByUsername(It.IsAny<string>())).Returns(Task.FromResult(true));

            var services = new ServiceCollection();
            services.AddTransient(_ => userRepo.Object);
            services.AddSingleton(typeof(ILogger<>), typeof(NullLogger<>));
            
            services.AddMediatR(IdentityDomain.Assembly);
            services.AddValidatorsFromAssembly(IdentityDomain.Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CommandValidationPipelineBehavior<,>));

            var provider = services.BuildServiceProvider();
            var mediatR = provider.GetRequiredService<IMediator>();

            var registerUserCommand = new RegisterUserCommand
            {
                NewUser = new NewUserDTO
                {
                    Email = "solo@yolo.com",
                    Username = "soloyolo",
                    Password = "soloyolo"
                }
            };

            //act
            var result = await mediatR.Send(registerUserCommand);
            
            //assert
            Assert.False(result.IsValidResponse);
            Assert.Equal("Username is already in use", result.ErrorMessage);
        }
        
        [Fact]
        public async Task GivenAnEmailAlreadyInUse_WhenRegistered_ThenFailsValidation()
        {
            //arrange
            var random = new Random();
            var userId = random.Next(1, 1000);
            
            var userRepo = new Mock<IUserRepository>();
            userRepo.Setup(repository => repository.Create(It.IsAny<User>())).Returns(Task.FromResult(userId));
            userRepo.Setup(repository => repository.ExistsByEmail(It.IsAny<string>())).Returns(Task.FromResult(true));
            userRepo.Setup(repository => repository.ExistsByUsername(It.IsAny<string>())).Returns(Task.FromResult(false));

            var services = new ServiceCollection();
            services.AddTransient(_ => userRepo.Object);
            services.AddSingleton(typeof(ILogger<>), typeof(NullLogger<>));
            
            services.AddMediatR(IdentityDomain.Assembly);
            services.AddValidatorsFromAssembly(IdentityDomain.Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CommandValidationPipelineBehavior<,>));

            var provider = services.BuildServiceProvider();
            var mediatR = provider.GetRequiredService<IMediator>();

            var registerUserCommand = new RegisterUserCommand
            {
                NewUser = new NewUserDTO
                {
                    Email = "solo@yolo.com",
                    Username = "soloyolo",
                    Password = "soloyolo"
                }
            };

            //act
            var result = await mediatR.Send(registerUserCommand);
            
            //assert
            Assert.False(result.IsValidResponse);
            Assert.Equal("Email is already in use", result.ErrorMessage);
        }
    }
}