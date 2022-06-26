using System.Threading.Tasks;
using Application.Core.PipelineBehaviors.OperationResponse;
using Application.Core.Testing;
using Application.Social.Domain.Contracts.Operations.Queries.GetProfile;
using Application.Social.Domain.Entities;
using Application.Social.Domain.Tests.Unit.Setup;
using AutoFixture;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Application.Social.Domain.Tests.Unit.Queries
{
    [Collection(nameof(SocialModuleTestCollection))]
    public class GetProfileTests : TestBase
    {
        private readonly SocialModuleSetupFixture _socialModule;
        
        public GetProfileTests(SocialModuleSetupFixture socialModule, ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _socialModule = socialModule;
            _socialModule.UserRepository.DeleteAll().GetAwaiter().GetResult();
        }

        [Fact]
        public async Task GivenAUsernameWhenGetProfileThenReturnsProfile()
        {
            //arrange
            var authenticatedUser = new UserEntity
            {
                Id = _socialModule.AuthenticatedUserId,
                Bio = _socialModule.AuthenticatedUserBio,
                Image = _socialModule.AuthenticatedUserImage,
                Username = _socialModule.AuthenticatedUserUsername
            };
            await _socialModule.UserRepository.Create(authenticatedUser);
            await _socialModule.UserRepository.FollowUser(authenticatedUser.Id);

            var otherUser = new UserEntity
            {
                Id = _socialModule.AutoFixture.Create<int>(),
                Bio = _socialModule.AutoFixture.Create<string>(),
                Image = _socialModule.AutoFixture.Create<string>(),
                Username = _socialModule.AutoFixture.Create<string>()
            };
            await _socialModule.UserRepository.Create(otherUser);
            await _socialModule.UserRepository.FollowUser(otherUser.Id);
            
            var otherUser2 = new UserEntity
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

        private void AssertFollowingUser(OperationResponse<GetProfileQueryResult> result, UserEntity userEntity, bool shouldBeFollowing)
        {
            result.Should().NotBeNull();
            result.Result.Should().Be(OperationResult.Success);
            result.Response.Should().NotBeNull();
            result.Response.Profile.Should().NotBeNull();
            result.Response.Profile.Username.Should().Be(userEntity.Username);
            result.Response.Profile.Image.Should().Be(userEntity.Image);
            result.Response.Profile.Bio.Should().Be(userEntity.Bio);
            result.Response.Profile.Following.Should().Be(shouldBeFollowing);
        }
    }
}