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
            //arrange
            _module.WithAuthenticatedUserContext();

            //act
            var result = await _module.Mediator.Send(_getCurrentUserQuery);
            
            //assert
            result.Result.Should().Be(OperationResult.Success);
            var currentUser = result.Response.CurrentUser;
            currentUser.Should().NotBeNull();
            
            //need to implement equality comparison between DTOs and Entities so we can just do Equals and have it update automatically without tests missing anything
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
        public async Task GivenNonExistentUser_WhenGetCurrentUser_ThenFailsValidation()
        {
            //arrange
            _module.WithRandomUserContext();

            //act
            var result = await _module.Mediator.Send(_getCurrentUserQuery);
            
            //assert
            result.Result.Should().Be(OperationResult.ValidationError);
            result.Response.Should().BeNull();
        }
    }
}