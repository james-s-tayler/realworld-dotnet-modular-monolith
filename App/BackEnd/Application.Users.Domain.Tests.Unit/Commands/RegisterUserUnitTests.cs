using System.Threading.Tasks;
using Application.Core.PipelineBehaviors.OperationResponse;
using Application.Core.Testing;
using Application.Users.Domain.Contracts.DTOs;
using Application.Users.Domain.Contracts.Operations.Commands.RegisterUser;
using Application.Users.Domain.Tests.Unit.Setup;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Xunit;
using Xunit.Abstractions;

namespace Application.Users.Domain.Tests.Unit.Commands
{
    [Collection(nameof(UsersModuleTestCollection))]
    public class RegisterUserUnitTests : UnitTestBase
    {
        private readonly UsersModuleSetupFixture _module;
        private readonly RegisterUserCommand _registerUserCommand;

        public RegisterUserUnitTests(UsersModuleSetupFixture module, ITestOutputHelper testOutputHelper) : base(testOutputHelper, module)
        {
            _module = module;
            _registerUserCommand = new RegisterUserCommand
            {
                NewUser = new NewUserDTO
                {
                    Email = $"{_module.AutoFixture.Create<string>()}@{_module.AutoFixture.Create<string>()}.com",
                    Username = _module.AutoFixture.Create<string>(),
                    Password = _module.PlainTextPassword
                }
            };
        }
        
        [Fact]
        public async Task GivenANewUser_WhenRegisterUser_ThenNewUserReturned()
        {
            //arrange

            //act
            var result = await _module.Mediator.Send(_registerUserCommand);
            
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
            var result = await _module.Mediator.Send(_registerUserCommand);
            
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
            var result = await _module.Mediator.Send(_registerUserCommand);
            
            //assert
            result.Result.Should().Be(OperationResult.ValidationError);
            result.Response.Should().BeNull();
        }
        
        [Fact]
        public async Task GivenAPassword_WhenRegisterUser_ThenPasswordHashedWithBcrypt()
        {
            //arrange

            //act
            var registerUserResponse = await _module.Mediator.Send(_registerUserCommand);
            
            //assert
            registerUserResponse.Should().NotBeNull();
            registerUserResponse.Response.Should().NotBeNull();

            var registeredUser = await _module.UserRepository.GetById(registerUserResponse.Response.RegisteredUser.Id);
            
            var result = _module.PasswordHasher.VerifyHashedPassword(registeredUser,
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
            _registerUserCommand.NewUser.Username = _module.ExistingUserEntity.Username;

            //act
            var result = await _module.Mediator.Send(_registerUserCommand);
            
            //assert
            result.Result.Should().Be(OperationResult.ValidationError);
            result.ErrorMessage.Should().Contain("Username is already in use");
        }
        
        [Fact]
        public async Task GivenAnEmailAlreadyInUse_WhenRegisterUser_ThenFailsValidation()
        {
            //arrange
            _registerUserCommand.NewUser.Email = _module.ExistingUserEntity.Email;

            //act
            var result = await _module.Mediator.Send(_registerUserCommand);
            
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
            var result = await _module.Mediator.Send(_registerUserCommand);
            
            //assert
            result.Result.Should().Be(OperationResult.ValidationError);
        }
    }
}