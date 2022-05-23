using System.Threading.Tasks;
using AutoFixture;
using Conduit.Core.PipelineBehaviors;
using Conduit.Core.Testing;
using Conduit.Users.Domain.Contracts.Commands.LoginUser;
using Conduit.Users.Domain.Tests.Unit.Setup;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Conduit.Users.Domain.Tests.Unit.Commands
{
    [Collection(nameof(UsersModuleTestCollection))]
    public class LoginUserTests : TestBase
    {
        private readonly UsersModuleSetupFixture _usersModule;
        private readonly LoginUserCommand _loginUserCommand;
        
        public LoginUserTests(UsersModuleSetupFixture usersModule, ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _usersModule = usersModule;
            _usersModule.WithUnauthenticatedUserContext();
            _loginUserCommand = new LoginUserCommand
            {
                UserCredentials = new UserCredentialsDTO
                {
                    Email = _usersModule.ExistingUser.Email,
                    Password = _usersModule.PlainTextPassword
                }
            };
        }
        
        [Fact]
        public async Task GivenValidCredentials_WhenLoginUser_ThenUserIsAuthenticated()
        {
            //arrange

            //act
            var result = await _usersModule.Mediator.Send(_loginUserCommand);
            
            //assert
            result.Result.Should().Be(OperationResult.Success);
            result.Response.IsAuthenticated.Should().BeTrue();
            result.Response.LoggedInUser.Should().NotBeNull();
            
            var loggedInUser = result.Response.LoggedInUser;

            loggedInUser.Email.Should().Be(_usersModule.ExistingUser.Email);
            loggedInUser.Username.Should().Be(_usersModule.ExistingUser.Username);
            loggedInUser.Token.Should().NotBeEmpty();
        }
        
        [Fact]
        public async Task GivenIncorrectPassword_WhenLoginUser_ThenUserIsNotAuthenticated()
        {
            //arrange
            _loginUserCommand.UserCredentials.Password = _usersModule.AutoFixture.Create<string>();

            //act
            var result = await _usersModule.Mediator.Send(_loginUserCommand);
            
            //assert
            result.Result.Should().Be(OperationResult.Success);
            result.Response.IsAuthenticated.Should().BeFalse();
            result.Response.LoggedInUser.Should().BeNull();
        }
        
        [Fact]
        public async Task GivenEmailNotRegistered_WhenLoginUser_ThenValidationFails()
        {
            //arrange
            _loginUserCommand.UserCredentials.Email = _usersModule.AutoFixture.Create<string>();

            //act
            var result = await _usersModule.Mediator.Send(_loginUserCommand);
            
            //assert
            result.Result.Should().Be(OperationResult.ValidationError);
            result.Response.Should().BeNull();
        }
    }
}