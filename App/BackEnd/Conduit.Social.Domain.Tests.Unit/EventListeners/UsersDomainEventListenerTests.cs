using System.Threading.Tasks;
using AutoFixture;
using Conduit.Core.Testing;
using Conduit.Social.Domain.Tests.Unit.Setup;
using Conduit.Users.Domain.Contracts;
using Conduit.Users.Domain.Contracts.Commands.RegisterUser;
using Conduit.Users.Domain.Contracts.Commands.UpdateUser;
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
            await _socialModule.UserRepository.DeleteAll();
            
            var registerUserEvent = new RegisterUserCommandResult
            {
                RegisteredUser = new UserDTO
                {
                    
                    Email = $"{_socialModule.AutoFixture.Create<string>()}@{_socialModule.AutoFixture.Create<string>()}.com",
                    Id = _socialModule.AutoFixture.Create<int>(),
                    Username = _socialModule.AutoFixture.Create<string>(),
                    Token = _socialModule.AutoFixture.Create<string>()
                }
            };
            
            //act
            await _socialModule.Mediator.Publish(registerUserEvent);

            //assert
            var exists = await _socialModule.UserRepository.Exists(registerUserEvent.RegisteredUser.Id);
            exists.Should().BeTrue();
        }
        
        [Fact]
        public async Task GivenAnExistingUser_WhenUpdateUserEventFired_ThenUserGetsUpdated()
        {
            //arrange
            await _socialModule.UserRepository.DeleteAll();
            
            var registerUserEvent = new RegisterUserCommandResult
            {
                RegisteredUser = new UserDTO
                {
                    
                    Email = $"{_socialModule.AutoFixture.Create<string>()}@{_socialModule.AutoFixture.Create<string>()}.com",
                    Id = _socialModule.AutoFixture.Create<int>(),
                    Username = _socialModule.AutoFixture.Create<string>(),
                    Token = _socialModule.AutoFixture.Create<string>()
                }
            };
            
            await _socialModule.Mediator.Publish(registerUserEvent);
            
            var updateUserEvent = new UpdateUserCommandResult
            {
                UpdatedUser = new UserDTO
                {
                    Id = registerUserEvent.RegisteredUser.Id,
                    Email = $"{_socialModule.AutoFixture.Create<string>()}@{_socialModule.AutoFixture.Create<string>()}.com",
                    Username = _socialModule.AutoFixture.Create<string>(),
                    Token = _socialModule.AutoFixture.Create<string>(),
                    Bio = _socialModule.AutoFixture.Create<string>(),
                    Image = _socialModule.AutoFixture.Create<string>()
                }
            };
            
            //act
            await _socialModule.Mediator.Publish(updateUserEvent);

            //assert
            var updateUser = await _socialModule.UserRepository.GetById(registerUserEvent.RegisteredUser.Id);
            updateUser.Bio.Should().Be(updateUserEvent.UpdatedUser.Bio);
            updateUser.Image.Should().Be(updateUserEvent.UpdatedUser.Image);
            updateUser.Username.Should().Be(updateUserEvent.UpdatedUser.Username);
        }
    }
}