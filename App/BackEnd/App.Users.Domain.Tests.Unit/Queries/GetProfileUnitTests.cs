using System.Threading.Tasks;
using App.Core.PipelineBehaviors.OperationResponse;
using App.Core.Testing;
using App.Users.Domain.Contracts;
using App.Users.Domain.Contracts.Operations.Queries.GetProfile;
using App.Users.Domain.Entities;
using App.Users.Domain.Tests.Unit.Setup;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace App.Users.Domain.Tests.Unit.Queries
{
    [Collection(nameof(UsersModuleTestCollection))]
    public class GetProfileUnitTests : UnitTestBase
    {
        private readonly UsersModuleSetupFixture _module;
        
        public GetProfileUnitTests(UsersModuleSetupFixture module, ITestOutputHelper testOutputHelper) : base(testOutputHelper, module)
        {
            _module = module;
        }

        [Fact]
        public async Task GivenAnAuthenticatedUser_WhenGetOwnProfile_ThenFollowingSelf()
        {
            //arrange
            var getOwnProfileRequest = new GetProfileQuery { Username = _module.AuthenticatedUserUsername };
            
            //act
            var getOwnProfileResult = await _module.Mediator.Send(getOwnProfileRequest);
            
            //assert
            AssertFollowingUser(getOwnProfileResult, _module.ExistingUserEntity, true);
        }
        
        [Fact]
        public async Task GivenAnAuthenticatedUser_WhenGetFollowedProfile_ThenIsFollowing()
        {
            //arrange
            var getFollowedProfileRequest = new GetProfileQuery { Username = _module.FollowedUser.Username };
            
            //act
            var getFollowedProfileResult = await _module.Mediator.Send(getFollowedProfileRequest);
            
            //assert
            AssertFollowingUser(getFollowedProfileResult, _module.FollowedUser, true);
        }
        
        [Fact]
        public async Task GivenAnAuthenticatedUser_WhenGetUnfollowedProfile_ThenIsNotFollowing()
        {
            //arrange
            var getUnfollowedProfileRequest = new GetProfileQuery { Username = _module.UnfollowedUser.Username };
            
            //act
            var getUnfollowedProfileResult = await _module.Mediator.Send(getUnfollowedProfileRequest);
            
            //assert
            AssertFollowingUser(getUnfollowedProfileResult, _module.UnfollowedUser, false);
        }
        
        [Fact]
        public async Task GivenAnUnauthenticatedUser_WhenGetAnyProfile_ThenIsNotFollowing()
        {
            //arrange
            _module.WithUnauthenticatedUserContext();
            
            var getFirstProfileRequest = new GetProfileQuery { Username = _module.AuthenticatedUserUsername };
            var getSecondProfileRequest = new GetProfileQuery { Username = _module.FollowedUser.Username };
            var getThirdProfileRequest = new GetProfileQuery { Username = _module.UnfollowedUser.Username };
            
            //act
            var getFirstProfileResult = await _module.Mediator.Send(getFirstProfileRequest);
            var getSecondProfileResult = await _module.Mediator.Send(getSecondProfileRequest);
            var getThirdProfileResult = await _module.Mediator.Send(getThirdProfileRequest);
            
            //assert
            AssertFollowingUser(getFirstProfileResult, _module.ExistingUserEntity, false);
            AssertFollowingUser(getSecondProfileResult, _module.FollowedUser, false);
            AssertFollowingUser(getThirdProfileResult, _module.UnfollowedUser, false);
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