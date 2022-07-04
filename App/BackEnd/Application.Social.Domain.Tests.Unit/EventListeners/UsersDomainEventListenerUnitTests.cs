using System.Threading.Tasks;
using Application.Core.Testing;
using Application.Social.Domain.Tests.Unit.Setup;
using Application.Users.Domain.Contracts.Operations.Commands.RegisterUser;
using Application.Users.Domain.Contracts.Operations.Commands.UpdateUser;
using AutoFixture;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Application.Social.Domain.Tests.Unit.EventListeners
{
    [Collection(nameof(SocialModuleTestCollection))]
    public class UsersDomainEventListenerUnitTests : UnitTestBase
    {
        private readonly SocialModuleSetupFixture _module;

        public UsersDomainEventListenerUnitTests(SocialModuleSetupFixture module, ITestOutputHelper testOutputHelper) : base(testOutputHelper, module)
        {
            _module = module;
        }

        [Fact]
        public async Task GivenUserRegisterEvent_WhenCheckUserRepository_ThenUserExists()
        {
            //arrange
            var registerUserEvent = _module.AutoFixture.Create<RegisterUserCommandResult>();
            registerUserEvent.RegisteredUser.Email = $"{_module.AutoFixture.Create<string>()}@{_module.AutoFixture.Create<string>()}.com";
            
            //act
            await _module.Mediator.Publish(registerUserEvent);

            //assert
            var exists = await _module.UserRepository.Exists(registerUserEvent.RegisteredUser.Id);
            exists.Should().BeTrue();
        }
        
        [Fact]
        public async Task GivenAnExistingUser_WhenUpdateUserEventFired_ThenUserGetsUpdated()
        {
            //arrange
            var registerUserEvent = _module.AutoFixture.Create<RegisterUserCommandResult>();
            registerUserEvent.RegisteredUser.Email = $"{_module.AutoFixture.Create<string>()}@{_module.AutoFixture.Create<string>()}.com";
            
            await _module.Mediator.Publish(registerUserEvent);

            var updateUserEvent = _module.AutoFixture.Create<UpdateUserCommandResult>();
            updateUserEvent.UpdatedUser.Id = registerUserEvent.RegisteredUser.Id;
            updateUserEvent.UpdatedUser.Email = $"{_module.AutoFixture.Create<string>()}@{_module.AutoFixture.Create<string>()}.com";

            //act
            await _module.Mediator.Publish(updateUserEvent);

            //assert
            var updateUser = await _module.UserRepository.GetById(registerUserEvent.RegisteredUser.Id);
            updateUser.Bio.Should().Be(updateUserEvent.UpdatedUser.Bio);
            updateUser.Image.Should().Be(updateUserEvent.UpdatedUser.Image);
            updateUser.Username.Should().Be(updateUserEvent.UpdatedUser.Username);
        }
    }
}