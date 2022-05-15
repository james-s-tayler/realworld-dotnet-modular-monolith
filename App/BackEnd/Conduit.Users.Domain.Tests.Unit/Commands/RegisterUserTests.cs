using System.Threading.Tasks;
using AutoFixture;
using Conduit.Core.PipelineBehaviors;
using Conduit.Identity.Domain.Contracts.Commands.RegisterUser;
using Conduit.Identity.Domain.Entities;
using Conduit.Identity.Domain.Tests.Unit.Setup;
using FluentAssertions;
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
        private readonly RegisterUserCommand _registerUserCommand;

        public RegisterUserTests(UsersModuleSetupFixture usersModule)
        {
            _usersModule = usersModule;
            _registerUserCommand = new RegisterUserCommand
            {
                NewUser = new NewUserDTO
                {
                    Email = _usersModule.AutoFixture.Create<string>(),
                    Username = _usersModule.AutoFixture.Create<string>(),
                    Password = _usersModule.PlainTextPassword
                }
            };
            _newUserId = _usersModule.AutoFixture.Create<int>();
            _registeredUser = null;
            _usersModule.UserRepo.Setup(repository => repository.Create(It.IsAny<User>()))
                .Callback<User>(u => _registeredUser = u)
                .Returns(Task.FromResult(_newUserId));
        }
        
        [Fact]
        public async Task GivenANewUser_WhenRegisterUser_ThenNewUserIdReturned()
        {
            //arrange

            //act
            var result = await _usersModule.Mediator.Send(_registerUserCommand);
            
            //assert
            result.Result.Should().Be(OperationResult.Success);
            result.Response.UserId.Should().Be(_newUserId);
            _registeredUser.Should().NotBeNull();
            _registeredUser.Email.Should().Be(_registerUserCommand.NewUser.Email);
            _registeredUser.Username.Should().Be(_registerUserCommand.NewUser.Username);
        }
        
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task GivenInvalidEmail_WhenRegisterUser_ThenFailsValidation(string email)
        {
            //arrange
            _registerUserCommand.NewUser.Email = email;

            //act
            var result = await _usersModule.Mediator.Send(_registerUserCommand);
            
            //assert
            result.Result.Should().Be(OperationResult.ValidationError);
            result.Response.Should().BeNull();
        }
        
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task GivenInvalidUsername_WhenRegisterUser_ThenFailsValidation(string username)
        {
            //arrange
            _registerUserCommand.NewUser.Username = username;

            //act
            var result = await _usersModule.Mediator.Send(_registerUserCommand);
            
            //assert
            result.Result.Should().Be(OperationResult.ValidationError);
            result.Response.Should().BeNull();
        }
        
        [Fact]
        public async Task GivenAPassword_WhenRegisterUser_ThenPasswordHashedWithBcrypt()
        {
            //arrange

            //act
            _ = await _usersModule.Mediator.Send(_registerUserCommand);
            var result = _usersModule.PasswordHasher.VerifyHashedPassword(_registeredUser,
                            _registeredUser.Password,
                            _registerUserCommand.NewUser.Password);

            //assert
            _registeredUser.Password.Should().NotBe(_registerUserCommand.NewUser.Password);
            result.Should().Be(PasswordVerificationResult.Success);
            
        }
        
        [Fact]
        public async Task GivenAUsernameAlreadyInUse_WhenRegisterUser_ThenFailsValidation()
        {
            //arrange
            _registerUserCommand.NewUser.Username = _usersModule.ExistingUser.Username;

            //act
            var result = await _usersModule.Mediator.Send(_registerUserCommand);
            
            //assert
            result.Result.Should().Be(OperationResult.ValidationError);
            result.ErrorMessage.Should().Be("Username is already in use");
        }
        
        [Fact]
        public async Task GivenAnEmailAlreadyInUse_WhenRegisterUser_ThenFailsValidation()
        {
            //arrange
            _registerUserCommand.NewUser.Email = _usersModule.ExistingUser.Email;

            //act
            var result = await _usersModule.Mediator.Send(_registerUserCommand);
            
            //assert
            result.Result.Should().Be(OperationResult.ValidationError);
            result.ErrorMessage.Should().Be("Email is already in use");
        }
        
        [Theory]
        [InlineData("1234567")]
        [InlineData("")]
        [InlineData(null)]
        public async Task GivenAnInvalidPassword_WhenRegisterUser_ThenFailsValidation(string invalidPassword)
        {
            //arrange
            _registerUserCommand.NewUser.Password = invalidPassword;

            //act
            var result = await _usersModule.Mediator.Send(_registerUserCommand);
            
            //assert
            result.Result.Should().Be(OperationResult.ValidationError);
        }
    }
}