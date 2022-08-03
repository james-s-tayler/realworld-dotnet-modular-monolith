using System.Linq;
using System.Threading.Tasks;
using App.Core.Testing;
using App.Feed.Domain.Tests.Unit.Setup;
using App.Users.Domain.Contracts.DTOs;
using App.Users.Domain.Contracts.Operations.Commands.FollowUser;
using App.Users.Domain.Contracts.Operations.Commands.UnfollowUser;
using AutoFixture;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace App.Feed.Domain.Tests.Unit.EventListeners
{
    [Collection(nameof(FeedModuleTestCollection))]
    public class UsersDomainEventListenerUnitTests : UnitTestBase
    {
        private readonly FeedModuleSetupFixture _module;

        public UsersDomainEventListenerUnitTests(FeedModuleSetupFixture module, ITestOutputHelper testOutputHelper) : base(testOutputHelper, module)
        {
            _module = module;
        }

        [Fact]
        public async Task GivenFollowUserEvent_WhenCheckFollowRepository_ThenFollowExists()
        {
            //arrange
            var followUserEvent = new FollowUserCommandResult
            {
                UserId = _module.AuthenticatedUserId,
                FollowingUserId = _module.AutoFixture.Create<int>(),
                FollowedProfile = _module.AutoFixture.Create<ProfileDTO>()
            };

            //act
            await _module.Mediator.Publish(followUserEvent);

            //assert
            var exists = await _module.FollowRepository.IsFollowing(followUserEvent.UserId, followUserEvent.FollowingUserId);
            exists.Should().BeTrue();
        }

        [Fact]
        public async Task GivenUnfollowUserEvent_WhenCheckFollowRepository_ThenFollowDeleted()
        {
            //arrange
            var unfollowUserEvent = new UnfollowUserCommandResult
            {
                UserId = _module.AuthenticatedUserId,
                FollowingUserId = _module.Follows.First(follow => follow.FollowingUserId != _module.AuthenticatedUserId).FollowingUserId,
                UnfollowedProfile = _module.AutoFixture.Create<ProfileDTO>()
            };

            //act
            await _module.Mediator.Publish(unfollowUserEvent);

            //assert
            var exists = await _module.FollowRepository.IsFollowing(unfollowUserEvent.UserId, unfollowUserEvent.FollowingUserId);
            exists.Should().BeFalse();
        }
    }
}