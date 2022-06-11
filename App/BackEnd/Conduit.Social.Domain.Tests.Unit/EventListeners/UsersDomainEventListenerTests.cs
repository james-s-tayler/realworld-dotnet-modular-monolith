using System.Threading.Tasks;
using Conduit.Core.Testing;
using Conduit.Social.Domain.Tests.Unit.Setup;
using Conduit.Users.Domain.Contracts;
using Conduit.Users.Domain.Contracts.Commands.RegisterUser;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Conduit.Social.Domain.Tests.Unit.EventListeners
{
    [Collection(nameof(SocialModuleTestCollection))]
    public class UsersDomainEventListenerTests : TestBase
    {
        private readonly SocialModuleSetupFixture _socialModule;

        public UsersDomainEventListenerTests(SocialModuleSetupFixture socialModule, ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _socialModule = socialModule;
        }

        [Fact]
        public async Task GivenUserRegisterEvent_WhenCheckUserRepository_ThenUserExists()
        {
            //arrange
            var registerUserEvent = new RegisterUserCommandResult
            {
                RegisteredUser = new UserDTO
                {
                    
                    Email = _socialModule.AuthenticatedUserEmail,
                    Id = _socialModule.AuthenticatedUserId,
                    Username = _socialModule.AuthenticatedUserUsername,
                    Token = _socialModule.AuthenticatedUserToken
                }
            };
            
            //act
            await _socialModule.Mediator.Publish(registerUserEvent);

            //assert
            var exists = await _socialModule.UserRepository.Exists(_socialModule.AuthenticatedUserId);
            exists.Should().BeTrue();
        }
    }
}