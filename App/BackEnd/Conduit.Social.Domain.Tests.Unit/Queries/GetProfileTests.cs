using System.Threading.Tasks;
using Application.Core.PipelineBehaviors.OperationResponse;
using Application.Core.Testing;
using AutoFixture;
using Conduit.Social.Domain.Contracts;
using Conduit.Social.Domain.Contracts.Queries.GetProfile;
using Conduit.Social.Domain.Entities;
using Conduit.Social.Domain.Tests.Unit.Setup;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Conduit.Social.Domain.Tests.Unit.Queries
{
    [Collection(nameof(SocialModuleTestCollection))]
    public class GetProfileTests : TestBase
    {
        private readonly SocialModuleSetupFixture _socialModule;
        
        public GetProfileTests(SocialModuleSetupFixture socialModule, ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _socialModule = socialModule;
        }

        [Fact]
        public async Task GivenAUsernameWhenGetProfileThenReturnsProfile()
        {
            await _socialModule.UserRepository.DeleteAll();
            
            //arrange
            var authenticatedUser = new User
            {
                Id = _socialModule.AuthenticatedUserId,
                Bio = _socialModule.AuthenticatedUserBio,
                Image = _socialModule.AuthenticatedUserImage,
                Username = _socialModule.AuthenticatedUserUsername
            };
            await _socialModule.UserRepository.Create(authenticatedUser);
            await _socialModule.UserRepository.FollowUser(authenticatedUser.Id);

            var otherUser = new User
            {
                Id = _socialModule.AutoFixture.Create<int>(),
                Bio = _socialModule.AutoFixture.Create<string>(),
                Image = _socialModule.AutoFixture.Create<string>(),
                Username = _socialModule.AutoFixture.Create<string>()
            };
            await _socialModule.UserRepository.Create(otherUser);
            await _socialModule.UserRepository.FollowUser(otherUser.Id);
            
            var otherUser2 = new User
            {
                Id = _socialModule.AutoFixture.Create<int>(),
                Bio = _socialModule.AutoFixture.Create<string>(),
                Image = _socialModule.AutoFixture.Create<string>(),
                Username = _socialModule.AutoFixture.Create<string>()
            };
            await _socialModule.UserRepository.Create(otherUser2);

            var getOwnProfileRequest = new GetProfileQuery { Username = _socialModule.AuthenticatedUserUsername };
            var getOtherUserProfileRequest = new GetProfileQuery { Username = otherUser.Username };
            var getOtherUser2ProfileRequest = new GetProfileQuery { Username = otherUser2.Username };

            //act
            var getOwnProfileResult = await _socialModule.Mediator.Send(getOwnProfileRequest);
            var getOtherUserProfileResult = await _socialModule.Mediator.Send(getOtherUserProfileRequest);
            var getOtherUser2ProfileResult = await _socialModule.Mediator.Send(getOtherUser2ProfileRequest);

            //assert
            AssertFollowingUser(getOwnProfileResult, authenticatedUser, true);
            AssertFollowingUser(getOtherUserProfileResult, otherUser, true);
            AssertFollowingUser(getOtherUser2ProfileResult, otherUser2, false);
        }

        private void AssertFollowingUser(OperationResponse<GetProfileQueryResult> result, User user, bool shouldBeFollowing)
        {
            result.Should().NotBeNull();
            result.Result.Should().Be(OperationResult.Success);
            result.Response.Should().NotBeNull();
            result.Response.Profile.Should().NotBeNull();
            result.Response.Profile.Username.Should().Be(user.Username);
            result.Response.Profile.Image.Should().Be(user.Image);
            result.Response.Profile.Bio.Should().Be(user.Bio);
            result.Response.Profile.Following.Should().Be(shouldBeFollowing);
        }
    }
}