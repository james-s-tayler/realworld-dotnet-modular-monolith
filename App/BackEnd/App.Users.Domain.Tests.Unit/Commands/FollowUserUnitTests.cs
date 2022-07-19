using System.Threading.Tasks;
using App.Core.PipelineBehaviors.OperationResponse;
using App.Core.Testing;
using App.Users.Domain.Contracts.DTOs;
using App.Users.Domain.Contracts.Operations.Commands.FollowUser;
using App.Users.Domain.Contracts.Operations.Commands.LoginUser;
using App.Users.Domain.Tests.Unit.Setup;
using AutoFixture;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace App.Users.Domain.Tests.Unit.Commands
{
    [Collection(nameof(UsersModuleTestCollection))]
    public class FollowUserUnitTests : UnitTestBase
    {
        private readonly UsersModuleSetupFixture _module;
        
        public FollowUserUnitTests(UsersModuleSetupFixture module, ITestOutputHelper testOutputHelper) : base(testOutputHelper, module)
        {
            _module = module;
        }
        
        [Fact]
        public async Task GivenAnUnfollowedUser_WhenFollowUser_ThenUserFollowed()
        {
            //arrange
            var followUserCommand = new FollowUserCommand
            {
                Username = _module.UnfollowedUser.Username 
            };
            
            //act
            var result = await _module.Mediator.Send(followUserCommand);
            
            //assert
            result.Result.Should().Be(OperationResult.Success);
            result.Response.Should().NotBeNull();
            result.Response.FollowedProfile.Username.Should().Be(followUserCommand.Username);
            result.Response.FollowedProfile.Following.Should().BeTrue();
        }
        
        [Fact]
        public async Task GivenAFollowedUser_WhenFollowUser_ThenUserStillFollowed()
        {
            //arrange
            var followUserCommand = new FollowUserCommand
            {
                Username = _module.FollowedUser.Username
            };
            
            //act
            var result = await _module.Mediator.Send(followUserCommand);
            
            //assert
            result.Result.Should().Be(OperationResult.Success);
            result.Response.Should().NotBeNull();
            result.Response.FollowedProfile.Username.Should().Be(followUserCommand.Username);
            result.Response.FollowedProfile.Following.Should().BeTrue();
        }
        
        [Fact]
        public async Task GivenNoUsername_WhenFollowUser_ThenInvalidRequest()
        {
            //arrange
            var followUserCommand = new FollowUserCommand
            {
                Username = null
            };
            
            //act
            var result = await _module.Mediator.Send(followUserCommand);
            
            //assert
            result.Result.Should().Be(OperationResult.InvalidRequest);
            result.Response.Should().BeNull();
        }
        
        [Fact]
        public async Task GivenNonExistentUser_WhenFollowUser_ThenValidationError()
        {
            //arrange
            var followUserCommand = new FollowUserCommand
            {
                Username = _module.AutoFixture.Create<string>()
            };
            
            //act
            var result = await _module.Mediator.Send(followUserCommand);
            
            //assert
            result.Result.Should().Be(OperationResult.ValidationError);
            result.Response.Should().BeNull();
        }
        
        [Fact]
        public async Task GivenAnUnauthenticatedUser_WhenFollowUser_ThenNotAuthenticated()
        {
            //arrange
            _module.WithUnauthenticatedUserContext();
            
            var followUserCommand = new FollowUserCommand
            {
                Username = _module.UnfollowedUser.Username
            };
            
            //act
            var result = await _module.Mediator.Send(followUserCommand);
            
            //assert
            result.Result.Should().Be(OperationResult.NotAuthenticated);
            result.Response.Should().BeNull();
        }
    }
}