using System.Threading.Tasks;
using App.Core.PipelineBehaviors.OperationResponse;
using App.Core.Testing;
using App.Users.Domain.Contracts.DTOs;
using App.Users.Domain.Contracts.Operations.Commands.FollowUser;
using App.Users.Domain.Contracts.Operations.Commands.LoginUser;
using App.Users.Domain.Contracts.Operations.Commands.UnfollowUser;
using App.Users.Domain.Tests.Unit.Setup;
using AutoFixture;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace App.Users.Domain.Tests.Unit.Commands
{
    [Collection(nameof(UsersModuleTestCollection))]
    public class UnfollowUserUnitTests : UnitTestBase
    {
        private readonly UsersModuleSetupFixture _module;

        public UnfollowUserUnitTests(UsersModuleSetupFixture module, ITestOutputHelper testOutputHelper) : base(testOutputHelper, module)
        {
            _module = module;
        }

        [Fact]
        public async Task GivenAFollowedUser_WhenUnfollowUser_ThenUserNotFollowed()
        {
            //arrange
            var unfollowUserCommand = new UnfollowUserCommand
            {
                Username = _module.FollowedUser.Username
            };

            //act
            var result = await _module.Mediator.Send(unfollowUserCommand);

            //assert
            result.Result.Should().Be(OperationResult.Success);
            result.Response.Should().NotBeNull();
            result.Response.UnfollowedProfile.Username.Should().Be(unfollowUserCommand.Username);
            result.Response.UnfollowedProfile.Following.Should().BeFalse();
        }

        [Fact]
        public async Task GivenAnUnfollowedUser_WhenUnfollowUser_ThenUserStillNotFollowed()
        {
            //arrange
            var unfollowUserCommand = new UnfollowUserCommand
            {
                Username = _module.UnfollowedUser.Username
            };

            //act
            var result = await _module.Mediator.Send(unfollowUserCommand);

            //assert
            result.Result.Should().Be(OperationResult.Success);
            result.Response.Should().NotBeNull();
            result.Response.UnfollowedProfile.Username.Should().Be(unfollowUserCommand.Username);
            result.Response.UnfollowedProfile.Following.Should().BeFalse();
        }

        [Fact]
        public async Task GivenNoUsername_WhenUnfollowUser_ThenInvalidRequest()
        {
            //arrange
            var unfollowUserCommand = new UnfollowUserCommand
            {
                Username = null
            };

            //act
            var result = await _module.Mediator.Send(unfollowUserCommand);

            //assert
            result.Result.Should().Be(OperationResult.InvalidRequest);
            result.Response.Should().BeNull();
        }

        [Fact]
        public async Task GivenNonExistentUser_WhenUnfollowUser_ThenValidationError()
        {
            //arrange
            var unfollowUserCommand = new UnfollowUserCommand
            {
                Username = _module.AutoFixture.Create<string>()
            };

            //act
            var result = await _module.Mediator.Send(unfollowUserCommand);

            //assert
            result.Result.Should().Be(OperationResult.ValidationError);
            result.Response.Should().BeNull();
        }

        [Fact]
        public async Task GivenAnUnauthenticatedUser_WhenUnfollowUser_ThenNotAuthenticated()
        {
            //arrange
            _module.WithUnauthenticatedUserContext();

            var unfollowUserCommand = new UnfollowUserCommand
            {
                Username = _module.FollowedUser.Username
            };

            //act
            var result = await _module.Mediator.Send(unfollowUserCommand);

            //assert
            result.Result.Should().Be(OperationResult.NotAuthenticated);
            result.Response.Should().BeNull();
        }
    }
}