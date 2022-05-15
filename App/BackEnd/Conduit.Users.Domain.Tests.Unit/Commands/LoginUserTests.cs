using System.Threading.Tasks;
using Conduit.Core.PipelineBehaviors;
using Conduit.Identity.Domain.Contracts.Commands.LoginUser;
using Conduit.Identity.Domain.Tests.Unit.Setup;
using Xunit;

namespace Conduit.Identity.Domain.Tests.Unit.Commands
{
    public class LoginUserTests : IClassFixture<ModuleSetupFixture>
    {
        private readonly ModuleSetupFixture _module;
        
        public LoginUserTests(ModuleSetupFixture module)
        {
            _module = module;
        }
        
        [Fact]
        public async Task GivenValidCredentials_WhenLoginUser_ThenUserIsAuthenticated()
        {
            //arrange
            var loginUserCommand = new LoginUserCommand
            {
                UserCredentials = new UserCredentialsDTO
                {
                    Email = _module.User.Email,
                    Password = _module.PlainTextPassword
                }
            };

            //act
            var result = await _module.Mediator.Send(loginUserCommand);
            
            //assert
            Assert.True(result.Result == OperationResult.Success);
            Assert.True(result.Response.IsAuthenticated);
            var loggedInUser = result.Response.LoggedInUser;
            Assert.NotNull(loggedInUser);
            Assert.Equal(_module.User.Email, loggedInUser.Email);
            Assert.Equal(_module.User.Username, loggedInUser.Username);
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
                    Email = _module.User.Email,
                    Password = "incorrectPassword"
                }
            };

            //act
            var result = await _module.Mediator.Send(loginUserCommand);
            
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
                    Email = "hello@example.com",
                    Password = _module.PlainTextPassword
                }
            };

            //act
            var result = await _module.Mediator.Send(loginUserCommand);
            
            //assert
            Assert.True(result.Result == OperationResult.ValidationError);
            Assert.Null(result.Response);
        }
    }
}