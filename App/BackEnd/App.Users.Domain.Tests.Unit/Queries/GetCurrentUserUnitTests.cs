using System.Threading.Tasks;
using App.Core.PipelineBehaviors.OperationResponse;
using App.Core.Testing;
using App.Users.Domain.Contracts.Operations.Queries.GetCurrentUser;
using App.Users.Domain.Tests.Unit.Setup;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace App.Users.Domain.Tests.Unit.Queries
{
    [Collection(nameof(UsersModuleTestCollection))]
    public class GetCurrentUserUnitTests : UnitTestBase
    {
        private readonly UsersModuleSetupFixture _module;
        private readonly GetCurrentUserQuery _getCurrentUserQuery;
        
        public GetCurrentUserUnitTests(UsersModuleSetupFixture module, ITestOutputHelper testOutputHelper) : base(testOutputHelper, module)
        {
            _module = module;
            _getCurrentUserQuery = new GetCurrentUserQuery();
        }

        [Fact]
        public async Task GivenAuthenticatedUser_WhenGetCurrentUser_ThenUserIsReturned()
        {
            //act
            var result = await _module.Mediator.Send(_getCurrentUserQuery);
            
            //assert
            result.Result.Should().Be(OperationResult.Success);
            var currentUser = result.Response.CurrentUser;
            currentUser.Should().NotBeNull();
            
            currentUser.Email.Should().Be(_module.ExistingUserEntity.Email);
            currentUser.Username.Should().Be(_module.ExistingUserEntity.Username);
            currentUser.Image.Should().Be(_module.ExistingUserEntity.Image);
            currentUser.Bio.Should().Be(_module.ExistingUserEntity.Bio);
            currentUser.Token.Should().NotBeEmpty();
        }
        
        [Fact]
        public async Task GivenUnauthenticatedUser_WhenGetCurrentUser_ThenNotAuthenticated()
        {
            //arrange
            _module.WithUnauthenticatedUserContext();

            //act
            var result = await _module.Mediator.Send(_getCurrentUserQuery);
            
            //assert
            result.Result.Should().Be(OperationResult.NotAuthenticated);
            result.Response.Should().BeNull();
        }
        
        [Fact]
        public async Task GivenNonExistentUser_WhenGetCurrentUser_ThenNotFound()
        {
            //arrange
            _module.WithAuthenticatedUserContext();

            //act
            var result = await _module.Mediator.Send(_getCurrentUserQuery);
            
            //assert
            result.Result.Should().Be(OperationResult.NotFound);
            result.Response.Should().BeNull();
        }
    }
}