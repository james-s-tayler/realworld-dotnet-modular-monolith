using System.Threading.Tasks;
using Application.Core.PipelineBehaviors.OperationResponse;
using Application.Core.Testing;
using Application.Users.Domain.Contracts.Operations.Queries.GetCurrentUser;
using Application.Users.Domain.Tests.Unit.Setup;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Application.Users.Domain.Tests.Unit.Queries
{
    [Collection(nameof(UsersModuleTestCollection))]
    public class GetCurrentUserTests : TestBase
    {
        private readonly UsersModuleSetupFixture _usersModule;
        private readonly GetCurrentUserQuery _getCurrentUserQuery;
        
        public GetCurrentUserTests(UsersModuleSetupFixture usersModule, ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _usersModule = usersModule;
            _getCurrentUserQuery = new GetCurrentUserQuery();
            _usersModule.WithUserRepoContainingDefaultUsers().GetAwaiter().GetResult();
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
            currentUser.Email.Should().Be(_usersModule.ExistingUserEntity.Email);
            currentUser.Username.Should().Be(_usersModule.ExistingUserEntity.Username);
            currentUser.Image.Should().Be(_usersModule.ExistingUserEntity.Image);
            currentUser.Bio.Should().Be(_usersModule.ExistingUserEntity.Bio);
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