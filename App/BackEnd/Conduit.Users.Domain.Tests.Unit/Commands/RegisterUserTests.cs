using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Conduit.Core.PipelineBehaviors;
using Conduit.Core.PipelineBehaviors.OperationResponse;
using Conduit.Core.Testing;
using Conduit.Users.Domain.Contracts.Commands.RegisterUser;
using Conduit.Users.Domain.Tests.Unit.Setup;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Xunit;
using Xunit.Abstractions;

namespace Conduit.Users.Domain.Tests.Unit.Commands
{
    [Collection(nameof(UsersModuleTestCollection))]
    public class RegisterUserTests : TestBase
    {
        private readonly UsersModuleSetupFixture _usersModule;
        private readonly RegisterUserCommand _registerUserCommand;

        public RegisterUserTests(UsersModuleSetupFixture usersModule, ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _usersModule = usersModule;
            _usersModule.WithUserRepoContainingDefaultUsers().GetAwaiter().GetResult();
            _registerUserCommand = new RegisterUserCommand
            {
                NewUser = new NewUserDTO
                {
                    Email = $"{_usersModule.AutoFixture.Create<string>()}@{_usersModule.AutoFixture.Create<string>()}.com",
                    Username = _usersModule.AutoFixture.Create<string>(),
                    Password = _usersModule.PlainTextPassword
                }
            };
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
            registeredUser.Id.Should().Be(3);
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
            var registerUserResponse = await _usersModule.Mediator.Send(_registerUserCommand);
            
            //assert
            registerUserResponse.Should().NotBeNull();
            registerUserResponse.Response.Should().NotBeNull();

            var registeredUser = await _usersModule.UserRepository.GetById(registerUserResponse.Response.RegisteredUser.Id);
            
            var result = _usersModule.PasswordHasher.VerifyHashedPassword(registeredUser,
                registeredUser.Password,
                _registerUserCommand.NewUser.Password);
            
            registeredUser.Should().NotBeNull();
            registeredUser.Password.Should().NotBeNullOrEmpty();
            registeredUser.Password.Should().NotBe(_registerUserCommand.NewUser.Password);
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
        [InlineData("123456789")]
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