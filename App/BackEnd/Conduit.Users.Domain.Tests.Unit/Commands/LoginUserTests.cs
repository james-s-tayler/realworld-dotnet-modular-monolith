using System.Threading.Tasks;
using AutoFixture;
using Conduit.Core.PipelineBehaviors;
using Conduit.Identity.Domain.Contracts.Commands.LoginUser;
using Conduit.Identity.Domain.Tests.Unit.Setup;
using Xunit;

namespace Conduit.Identity.Domain.Tests.Unit.Commands
{
    [Collection(nameof(UsersModuleTestCollection))]
    public class LoginUserTests
    {
        private readonly UsersModuleSetupFixture _usersModule;
        
        public LoginUserTests(UsersModuleSetupFixture usersModule)
        {
            _usersModule = usersModule;
            _usersModule.WithUnauthenticatedUserContext();
        }
        
        [Fact]
        public async Task GivenValidCredentials_WhenLoginUser_ThenUserIsAuthenticated()
        {
            //arrange
            var loginUserCommand = new LoginUserCommand
            {
                UserCredentials = new UserCredentialsDTO
                {
                    Email = _usersModule.ExistingUser.Email,
                    Password = _usersModule.PlainTextPassword
                }
            };

            //act
            var result = await _usersModule.Mediator.Send(loginUserCommand);
            
            //assert
            Assert.True(result.Result == OperationResult.Success);
            Assert.True(result.Response.IsAuthenticated);
            var loggedInUser = result.Response.LoggedInUser;
            Assert.NotNull(loggedInUser);
            Assert.Equal(_usersModule.ExistingUser.Email, loggedInUser.Email);
            Assert.Equal(_usersModule.ExistingUser.Username, loggedInUser.Username);
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
                    Email = _usersModule.ExistingUser.Email,
                    Password = _usersModule.AutoFixture.Create<string>()
                }
            };

            //act
            var result = await _usersModule.Mediator.Send(loginUserCommand);
            
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
                    Email = _usersModule.AutoFixture.Create<string>(),
                    Password = _usersModule.PlainTextPassword
                }
            };

            //act
            var result = await _usersModule.Mediator.Send(loginUserCommand);
            
            //assert
            Assert.True(result.Result == OperationResult.ValidationError);
            Assert.Null(result.Response);
        }
    }
}