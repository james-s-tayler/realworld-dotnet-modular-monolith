using System.Threading.Tasks;
using AutoFixture;
using Conduit.Core.PipelineBehaviors;
using Conduit.Identity.Domain.Contracts.Commands.RegisterUser;
using Conduit.Identity.Domain.Entities;
using Conduit.Identity.Domain.Tests.Unit.Setup;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace Conduit.Identity.Domain.Tests.Unit.Commands
{
    [Collection(nameof(UsersModuleTestCollection))]
    public class RegisterUserTests
    {
        private readonly UsersModuleSetupFixture _usersModule;
        private User _registeredUser;
        private readonly int _newUserId;

        public RegisterUserTests(UsersModuleSetupFixture usersModule)
        {
            _usersModule = usersModule;
            _newUserId = _usersModule.AutoFixture.Create<int>();
            _registeredUser = null;
            _usersModule.UserRepo.Setup(repository => repository.Create(It.IsAny<User>()))
                .Callback<User>(u => _registeredUser = u)
                .Returns(Task.FromResult(_newUserId));
        }
        
        //TODO: test password strength requirements are met

        [Fact]
        public async Task GivenANewUser_WhenRegisterUser_ThenNewUserIdReturned()
        {
            //arrange
            var registerUserCommand = new RegisterUserCommand
            {
                NewUser = new NewUserDTO
                {
                    Email = _usersModule.AutoFixture.Create<string>(),
                    Username = _usersModule.AutoFixture.Create<string>(),
                    Password = _usersModule.PlainTextPassword
                }
            };

            //act
            var result = await _usersModule.Mediator.Send(registerUserCommand);
            
            //assert
            Assert.True(result.Result == OperationResult.Success);
            Assert.Equal(_newUserId, result.Response.UserId);
            Assert.NotNull(_registeredUser);
            Assert.Equal(registerUserCommand.NewUser.Email, _registeredUser.Email);
            Assert.Equal(registerUserCommand.NewUser.Username, _registeredUser.Username);
        }
        
        [Fact]
        public async Task GivenAPassword_WhenRegisterUser_ThenPasswordHashedWithBcrypt()
        {
            //arrange
            var registerUserCommand = new RegisterUserCommand
            {
                NewUser = new NewUserDTO
                {
                    Email = _usersModule.AutoFixture.Create<string>(),
                    Username = _usersModule.AutoFixture.Create<string>(),
                    Password = _usersModule.PlainTextPassword
                }
            };

            //act
            _ = await _usersModule.Mediator.Send(registerUserCommand);

            //assert
            Assert.NotEqual(registerUserCommand.NewUser.Password, _registeredUser.Password);
            var result = _usersModule.PasswordHasher.VerifyHashedPassword(_registeredUser, _registeredUser.Password, registerUserCommand.NewUser.Password);

            Assert.Equal(PasswordVerificationResult.Success, result);
        }
        
        [Fact]
        public async Task GivenAUsernameAlreadyInUse_WhenRegisterUser_ThenFailsValidation()
        {
            //arrange
            var registerUserCommand = new RegisterUserCommand
            {
                NewUser = new NewUserDTO
                {
                    Email = _usersModule.AutoFixture.Create<string>(),
                    Username = _usersModule.ExistingUser.Username,
                    Password = _usersModule.PlainTextPassword
                }
            };

            //act
            var result = await _usersModule.Mediator.Send(registerUserCommand);
            
            //assert
            Assert.True(result.Result == OperationResult.ValidationError);
            Assert.Equal("Username is already in use", result.ErrorMessage);
        }
        
        [Fact]
        public async Task GivenAnEmailAlreadyInUse_WhenRegisterUser_ThenFailsValidation()
        {
            //arrange
            var registerUserCommand = new RegisterUserCommand
            {
                NewUser = new NewUserDTO
                {
                    Email = _usersModule.ExistingUser.Email,
                    Username = _usersModule.AutoFixture.Create<string>(),
                    Password = _usersModule.PlainTextPassword
                }
            };

            //act
            var result = await _usersModule.Mediator.Send(registerUserCommand);
            
            //assert
            Assert.True(result.Result == OperationResult.ValidationError);
            Assert.Equal("Email is already in use", result.ErrorMessage);
        }
    }
}