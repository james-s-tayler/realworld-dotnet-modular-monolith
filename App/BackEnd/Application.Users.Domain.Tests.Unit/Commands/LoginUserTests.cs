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
                    Email = _usersModule.ExistingUserEntity.Email,
                    Password = _usersModule.PlainTextPassword
                }
            };
            
            _usersModule.WithUserRepoContainingDefaultUsers().GetAwaiter().GetResult();
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

            loggedInUser.Email.Should().Be(_usersModule.ExistingUserEntity.Email);
            loggedInUser.Username.Should().Be(_usersModule.ExistingUserEntity.Username);
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