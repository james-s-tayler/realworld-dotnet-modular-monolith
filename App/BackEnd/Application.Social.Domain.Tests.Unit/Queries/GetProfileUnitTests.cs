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
    public class GetProfileUnitTests : UnitTestBase
    {
        private readonly SocialModuleSetupFixture _module;
        
        public GetProfileUnitTests(SocialModuleSetupFixture module, ITestOutputHelper testOutputHelper) : base(testOutputHelper, module)
        {
            _module = module;
        }

        [Fact]
        public async Task GivenAUsernameWhenGetProfileThenReturnsProfile()
        {
            //arrange
            var authenticatedUser = new UserEntity
            {
                Id = _module.AuthenticatedUserId,
                Bio = _module.AuthenticatedUserBio,
                Image = _module.AuthenticatedUserImage,
                Username = _module.AuthenticatedUserUsername
            };
            await _module.UserRepository.Create(authenticatedUser);
            await _module.UserRepository.FollowUser(authenticatedUser.Id);

            var otherUser = new UserEntity
            {
                Id = _module.AutoFixture.Create<int>(),
                Bio = _module.AutoFixture.Create<string>(),
                Image = _module.AutoFixture.Create<string>(),
                Username = _module.AutoFixture.Create<string>()
            };
            await _module.UserRepository.Create(otherUser);
            await _module.UserRepository.FollowUser(otherUser.Id);
            
            var otherUser2 = new UserEntity
            {
                Id = _module.AutoFixture.Create<int>(),
                Bio = _module.AutoFixture.Create<string>(),
                Image = _module.AutoFixture.Create<string>(),
                Username = _module.AutoFixture.Create<string>()
            };
            await _module.UserRepository.Create(otherUser2);

            var getOwnProfileRequest = new GetProfileQuery { Username = _module.AuthenticatedUserUsername };
            var getOtherUserProfileRequest = new GetProfileQuery { Username = otherUser.Username };
            var getOtherUser2ProfileRequest = new GetProfileQuery { Username = otherUser2.Username };

            //act
            var getOwnProfileResult = await _module.Mediator.Send(getOwnProfileRequest);
            var getOtherUserProfileResult = await _module.Mediator.Send(getOtherUserProfileRequest);
            var getOtherUser2ProfileResult = await _module.Mediator.Send(getOtherUser2ProfileRequest);

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