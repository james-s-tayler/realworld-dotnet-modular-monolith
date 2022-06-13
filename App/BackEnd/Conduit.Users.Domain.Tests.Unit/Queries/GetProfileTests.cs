using System.Threading.Tasks;
using Conduit.Core.PipelineBehaviors.OperationResponse;
using Conduit.Core.Testing;
using Conduit.Users.Domain.Contracts.Queries.GetProfile;
using Conduit.Users.Domain.Tests.Unit.Setup;
using Xunit;
using Xunit.Abstractions;
using FluentAssertions;

namespace Conduit.Users.Domain.Tests.Unit.Queries
{
    [Collection(nameof(UsersModuleTestCollection))]
    public class GetProfileTests : TestBase
    {
        private readonly UsersModuleSetupFixture _usersModule;
        
        public GetProfileTests(UsersModuleSetupFixture usersModule, ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _usersModule = usersModule;
        }

        [Fact]
        public async Task GivenAUsernameWhenGetProfileThenReturnsProfile()
        {
            //arrange
            var getProfileRequest = new GetProfileQuery { Username = _usersModule.ExistingUser2.Username };

            //act
            var result = await _usersModule.Mediator.Send(getProfileRequest);

            //assert
            result.Should().NotBeNull();
            result.Result.Should().Be(OperationResult.Success);
            result.Response.Should().NotBeNull();
            result.Response.Profile.Should().NotBeNull();
            result.Response.Profile.Username.Should().Be(_usersModule.ExistingUser2.Username);
            result.Response.Profile.Image.Should().Be(_usersModule.ExistingUser2.Image);
            result.Response.Profile.Bio.Should().Be(_usersModule.ExistingUser2.Bio);
            result.Response.Profile.Following.Should().BeFalse();
        }
    }
}