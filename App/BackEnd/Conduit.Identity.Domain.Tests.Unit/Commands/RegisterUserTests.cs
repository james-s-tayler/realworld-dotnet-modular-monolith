using System;
using System.Threading.Tasks;
using Conduit.Core.Validation;
using Conduit.Identity.Domain.Contracts.Commands.RegisterUser;
using Conduit.Identity.Domain.Entities;
using Conduit.Identity.Domain.Infrastructure.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using ScottBrady91.AspNetCore.Identity;
using Xunit;

namespace Conduit.Identity.Domain.Tests.Unit.Commands
{
    public class RegisterUserTests
    {
        //should refactor these tests to make the arrange phase less noisy - use either a fixture or helper methods

        //test password strength requirements are met
        
        [Fact]
        public async Task GivenANewUser_WhenRegisterUser_ThenNewUserIdReturned()
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
            services.AddTransient<IPasswordHasher<User>, BCryptPasswordHasher<User>>();
            
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
            Assert.True(result.Result == OperationResult.Success);
            Assert.Equal(userId, result.Response.UserId);
            Assert.NotNull(registeredUser);
            Assert.Equal(registerUserCommand.NewUser.Email, registeredUser.Email);
            Assert.Equal(registerUserCommand.NewUser.Username, registeredUser.Username);
        }
        
        [Fact]
        public async Task GivenAPassword_WhenRegisterUser_ThenPasswordHashedWithBcrypt()
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
            services.AddTransient<IPasswordHasher<User>, BCryptPasswordHasher<User>>();
            
            services.AddMediatR(IdentityDomain.Assembly);
            services.AddValidatorsFromAssembly(IdentityDomain.Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CommandValidationPipelineBehavior<,>));

            var provider = services.BuildServiceProvider();
            var mediatR = provider.GetRequiredService<IMediator>();
            var passwordHasher = provider.GetRequiredService<IPasswordHasher<User>>();

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
            _ = await mediatR.Send(registerUserCommand);

            //assert
            Assert.NotEqual(registerUserCommand.NewUser.Password, registeredUser.Password);
            var result = passwordHasher.VerifyHashedPassword(registeredUser, registeredUser.Password, registerUserCommand.NewUser.Password);

            Assert.Equal(PasswordVerificationResult.Success, result);
        }
        
        [Fact]
        public async Task GivenAUsernameAlreadyInUse_WhenRegisterUser_ThenFailsValidation()
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
            services.AddTransient<IPasswordHasher<User>, BCryptPasswordHasher<User>>();
            
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
            Assert.True(result.Result == OperationResult.ValidationError);
            Assert.Equal("Username is already in use", result.ErrorMessage);
        }
        
        [Fact]
        public async Task GivenAnEmailAlreadyInUse_WhenRegisterUser_ThenFailsValidation()
        {
            //arrange
            var random = new Random();
            var userId = random.Next(1, 1000);
            
            var userRepo = new Mock<IUserRepository>();
            userRepo.Setup(repository => repository.Create(It.IsAny<User>())).Returns(Task.FromResult(userId));
            userRepo.Setup(repository => repository.ExistsByEmail(It.IsAny<string>())).Returns(Task.FromResult(true));
            userRepo.Setup(repository => repository.ExistsByUsername(It.IsAny<string>())).Returns(Task.FromResult(false));

            /*var startup = new ModuleStartup();
            var services = new ServiceCollection();
            startup.AddServices(services);*/
            var services = new ServiceCollection();
            services.AddTransient(_ => userRepo.Object);
            services.AddSingleton(typeof(ILogger<>), typeof(NullLogger<>));
            services.AddTransient<IPasswordHasher<User>, BCryptPasswordHasher<User>>();
            
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
            Assert.True(result.Result == OperationResult.ValidationError);
            Assert.Equal("Email is already in use", result.ErrorMessage);
        }
    }
}