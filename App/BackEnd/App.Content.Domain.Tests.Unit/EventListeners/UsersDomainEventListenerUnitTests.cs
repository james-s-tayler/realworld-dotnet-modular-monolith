using System.Threading.Tasks;
using App.Content.Domain.Tests.Unit.Setup;
using App.Core.Testing;
using App.Users.Domain.Contracts.DTOs;
using App.Users.Domain.Contracts.Operations.Commands.RegisterUser;
using App.Users.Domain.Contracts.Operations.Commands.UpdateUser;
using AutoFixture;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace App.Content.Domain.Tests.Unit.EventListeners
{
    [Collection(nameof(ContentModuleTestCollection))]
    public class UsersDomainEventListenerUnitTests : UnitTestBase
    {
        private readonly ContentModuleSetupFixture _module;

        public UsersDomainEventListenerUnitTests(ContentModuleSetupFixture module, ITestOutputHelper testOutputHelper) : base(testOutputHelper, module)
        {
            _module = module;
            _module.UserRepository.DeleteAll().GetAwaiter().GetResult();
        }

        [Fact]
        public async Task GivenUserRegisterEvent_WhenCheckUserRepository_ThenUserExists()
        {
            //arrange
            var registerUserEvent = new RegisterUserCommandResult
            {
                RegisteredUser = new UserDTO
                {

                    Email = $"{_module.AutoFixture.Create<string>()}@{_module.AutoFixture.Create<string>()}.com",
                    Id = _module.AutoFixture.Create<int>(),
                    Username = _module.AutoFixture.Create<string>(),
                    Token = _module.AutoFixture.Create<string>()
                }
            };

            //act
            await _module.Mediator.Publish(registerUserEvent);

            //assert
            var exists = await _module.UserRepository.Exists(registerUserEvent.RegisteredUser.Id);
            var existsByUsername = await _module.UserRepository.ExistsByUsername(registerUserEvent.RegisteredUser.Username);
            exists.Should().BeTrue();
            existsByUsername.Should().BeTrue();
        }

        [Fact]
        public async Task GivenAnExistingUser_WhenUpdateUserEventFired_ThenUserGetsUpdated()
        {
            //arrange
            var registerUserEvent = new RegisterUserCommandResult
            {
                RegisteredUser = new UserDTO
                {

                    Email = $"{_module.AutoFixture.Create<string>()}@{_module.AutoFixture.Create<string>()}.com",
                    Id = _module.AutoFixture.Create<int>(),
                    Username = _module.AutoFixture.Create<string>(),
                    Token = _module.AutoFixture.Create<string>()
                }
            };

            await _module.Mediator.Publish(registerUserEvent);

            var updateUserEvent = new UpdateUserCommandResult
            {
                UpdatedUser = new UserDTO
                {
                    Id = registerUserEvent.RegisteredUser.Id,
                    Email = $"{_module.AutoFixture.Create<string>()}@{_module.AutoFixture.Create<string>()}.com",
                    Username = _module.AutoFixture.Create<string>(),
                    Token = _module.AutoFixture.Create<string>(),
                    Bio = _module.AutoFixture.Create<string>(),
                    Image = _module.AutoFixture.Create<string>()
                }
            };

            //act
            await _module.Mediator.Publish(updateUserEvent);

            //assert
            var updateUser = await _module.UserRepository.GetById(registerUserEvent.RegisteredUser.Id);
            updateUser.Username.Should().Be(updateUserEvent.UpdatedUser.Username);
        }
    }
}