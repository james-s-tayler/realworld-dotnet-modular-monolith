using System.Threading.Tasks;
using AutoFixture;
using Conduit.Core.PipelineBehaviors;
using Conduit.Core.Testing;
using Conduit.Users.Domain.Contracts.Commands.RegisterUser;
using Conduit.Users.Domain.Entities;
using Conduit.Users.Domain.Tests.Unit.Setup;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Conduit.Users.Domain.Tests.Unit.Commands
{
    [Collection(nameof(UsersModuleTestCollection))]
    public class RegisterUserTests : TestBase
    {
        private readonly UsersModuleSetupFixture _usersModule;
        private User _registeredUser;
        private readonly int _newUserId;
        private readonly RegisterUserCommand _registerUserCommand;

        public RegisterUserTests(UsersModuleSetupFixture usersModule, ITestOutputHelper testOutputHelper) : base(testOutputHelper)
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
                .Callback<User>(registeredUser =>
                {
                    //don't really love this - maybe better not to mock and just use the in memory repository or SQLite
                    registeredUser.Id = _newUserId;
                    _usersModule.AddUserToUserRepo(registeredUser);
                    _registeredUser = registeredUser;
                })
                .Returns(Task.FromResult(_newUserId));
        }
        
        [Fact]
        public async Task GivenANewUser_WhenRegisterUser_ThenNewUserReturned()
        {
            //arrange
            
            //act
            var result = await _usersModule.Mediator.Send(_registerUserCommand);
            
            //assert
            result.Result.Should().Be(OperationResult.Success);
            result.Response.Should().NotBeNull();
            result.Response.RegisteredUser.Should().NotBeNull();
            var registeredUser = result.Response.RegisteredUser;
            registeredUser.Id.Should().Be(_newUserId);
            registeredUser.Email.Should().Be(_registerUserCommand.NewUser.Email);
            registeredUser.Username.Should().Be(_registerUserCommand.NewUser.Username);
            registeredUser.Token.Should().NotBeNull();
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
            _registeredUser.Should().NotBeNull();
            _registeredUser.Password.Should().NotBeNullOrEmpty();
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
            result.ErrorMessage.Should().Contain("Username is already in use");
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
            result.ErrorMessage.Should().Contain("Email is already in use");
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