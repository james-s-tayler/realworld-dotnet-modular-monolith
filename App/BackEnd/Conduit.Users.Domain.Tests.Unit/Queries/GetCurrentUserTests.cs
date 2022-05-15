using System.Threading.Tasks;
using Conduit.Core.PipelineBehaviors;
using Conduit.Identity.Domain.Contracts.Queries.GetCurrentUser;
using Conduit.Identity.Domain.Tests.Unit.Setup;
using FluentAssertions;
using Xunit;

namespace Conduit.Identity.Domain.Tests.Unit.Queries
{
    [Collection(nameof(UsersModuleTestCollection))]
    public class GetCurrentUserTests
    {
        private readonly UsersModuleSetupFixture _usersModule;
        private readonly GetCurrentUserQuery _getCurrentUserQuery;
        
        public GetCurrentUserTests(UsersModuleSetupFixture usersModule)
        {
            _usersModule = usersModule;
            _getCurrentUserQuery = new GetCurrentUserQuery();
        }

        [Fact]
        public async Task GivenAuthenticatedUser_WhenGetCurrentUser_ThenUserIsReturned()
        {
            //arrange
            _usersModule.WithAuthenticatedUserContext();

            //act
            var result = await _usersModule.Mediator.Send(_getCurrentUserQuery);
            
            //assert
            result.Result.Should().Be(OperationResult.Success);
            var currentUser = result.Response.CurrentUser;
            currentUser.Should().NotBeNull();
            
            //need to implement equality comparison between DTOs and Entities so we can just do Equals and have it update automatically without tests missing anything
            currentUser.Email.Should().Be(_usersModule.ExistingUser.Email);
            currentUser.Username.Should().Be(_usersModule.ExistingUser.Username);
            currentUser.Image.Should().Be(_usersModule.ExistingUser.Image);
            currentUser.Bio.Should().Be(_usersModule.ExistingUser.Bio);
            currentUser.Token.Should().NotBeEmpty();
        }
        
        [Fact]
        public async Task GivenUnauthenticatedUser_WhenGetCurrentUser_ThenNotAuthenticated()
        {
            //arrange
            _usersModule.WithUnauthenticatedUserContext();

            //act
            var result = await _usersModule.Mediator.Send(_getCurrentUserQuery);
            
            //assert
            result.Result.Should().Be(OperationResult.NotAuthenticated);
            result.Response.Should().BeNull();
        }
        
        [Fact]
        public async Task GivenNonExistentUser_WhenGetCurrentUser_ThenFailsValidation()
        {
            //arrange
            _usersModule.WithRandomUserContext();

            //act
            var result = await _usersModule.Mediator.Send(_getCurrentUserQuery);
            
            //assert
            result.Result.Should().Be(OperationResult.ValidationError);
            result.Response.Should().BeNull();
        }
    }
}