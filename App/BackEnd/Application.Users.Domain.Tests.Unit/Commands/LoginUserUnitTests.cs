using System.Threading.Tasks;
using Application.Core.PipelineBehaviors.OperationResponse;
using Application.Core.Testing;
using Application.Users.Domain.Contracts.DTOs;
using Application.Users.Domain.Contracts.Operations.Commands.LoginUser;
using Application.Users.Domain.Tests.Unit.Setup;
using AutoFixture;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Application.Users.Domain.Tests.Unit.Commands
{
    [Collection(nameof(UsersModuleTestCollection))]
    public class LoginUserUnitTests : UnitTestBase
    {
        private readonly UsersModuleSetupFixture _module;
        private readonly LoginUserCommand _loginUserCommand;
        
        public LoginUserUnitTests(UsersModuleSetupFixture module, ITestOutputHelper testOutputHelper) : base(testOutputHelper, module)
        {
            _module = module;
            _loginUserCommand = new LoginUserCommand
            {
                UserCredentials = new UserCredentialsDTO
                {
                    Email = _module.ExistingUserEntity.Email,
                    Password = _module.PlainTextPassword
                }
            };
            
            _module.WithUnauthenticatedUserContext();
        }
        
        [Fact]
        public async Task GivenValidCredentials_WhenLoginUser_ThenUserIsAuthenticated()
        {
            //arrange

            //act
            var result = await _module.Mediator.Send(_loginUserCommand);
            
            //assert
            result.Result.Should().Be(OperationResult.Success);
            result.Response.IsAuthenticated.Should().BeTrue();
            result.Response.LoggedInUser.Should().NotBeNull();
            
            var loggedInUser = result.Response.LoggedInUser;

            loggedInUser.Email.Should().Be(_module.ExistingUserEntity.Email);
            loggedInUser.Username.Should().Be(_module.ExistingUserEntity.Username);
            loggedInUser.Token.Should().NotBeEmpty();
        }
        
        [Fact]
        public async Task GivenIncorrectPassword_WhenLoginUser_ThenUserIsNotAuthenticated()
        {
            //arrange
            _loginUserCommand.UserCredentials.Password = _module.AutoFixture.Create<string>();

            //act
            var result = await _module.Mediator.Send(_loginUserCommand);
            
            //assert
            result.Result.Should().Be(OperationResult.Success);
            result.Response.IsAuthenticated.Should().BeFalse();
            result.Response.LoggedInUser.Should().BeNull();
        }
        
        [Fact]
        public async Task GivenEmailNotRegistered_WhenLoginUser_ThenValidationFails()
        {
            //arrange
            _loginUserCommand.UserCredentials.Email = _module.AutoFixture.Create<string>();

            //act
            var result = await _module.Mediator.Send(_loginUserCommand);
            
            //assert
            result.Result.Should().Be(OperationResult.ValidationError);
            result.Response.Should().BeNull();
        }
    }
}