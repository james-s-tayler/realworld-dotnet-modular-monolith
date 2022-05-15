using System.Threading.Tasks;
using AutoFixture;
using Conduit.Core.PipelineBehaviors;
using Conduit.Identity.Domain.Contracts.Commands.UpdateUser;
using Conduit.Identity.Domain.Tests.Unit.Setup;
using Xunit;
using FluentAssertions;

namespace Conduit.Identity.Domain.Tests.Unit.Commands
{
    [Collection(nameof(UsersModuleTestCollection))]
    public class UpdateUserTests
    {
        private readonly UsersModuleSetupFixture _module;
        private readonly UpdateUserCommand _updateUserCommand;

        public UpdateUserTests(UsersModuleSetupFixture module)
        {
            _module = module;
            _module.WithAuthenticatedUserContext();
            _updateUserCommand = new UpdateUserCommand
            {
                UpdateUser = new UpdateUserDTO
                {
                    Bio = _module.AutoFixture.Create<string>(),
                    Email = _module.AutoFixture.Create<string>(),
                    Image = _module.AutoFixture.Create<string>(),
                    Username = _module.AutoFixture.Create<string>()
                }
            };
        }

        [Theory]
        [InlineData(true, true, true, true)]
        [InlineData(true, false, false, false)]
        [InlineData(false, true, false, false)]
        [InlineData(false, false, true, false)]
        [InlineData(false, false, false, true)]
        public async Task GivenPropertiesToUpdate_WhenUpdateUser_ThenPropertiesUpdated(bool updateUsername, bool updateEmail, bool updateImage, bool updateBio)
        {
            //arrange
            if(!updateUsername) _updateUserCommand.UpdateUser.Username = null;
            if(!updateEmail) _updateUserCommand.UpdateUser.Email = null;
            if(!updateImage) _updateUserCommand.UpdateUser.Image = null;
            if(!updateBio) _updateUserCommand.UpdateUser.Bio = null;

            //act
            var updateUserResult = await _module.Mediator.Send(_updateUserCommand);

            //assert
            updateUserResult.Result.Should().Be(OperationResult.Success);
            updateUserResult.Response.Should().NotBeNull();
            updateUserResult.Response.UpdatedUser.Id.Should().Be(_module.ExistingUser.Id);

            updateUserResult.Response.UpdatedUser.Username.Should().Be(updateUsername
                ? _updateUserCommand.UpdateUser.Username
                : _module.ExistingUser.Username);
            
            updateUserResult.Response.UpdatedUser.Email.Should().Be(updateEmail
                ? _updateUserCommand.UpdateUser.Email
                : _module.ExistingUser.Email);
            
            updateUserResult.Response.UpdatedUser.Image.Should().Be(updateImage
                ? _updateUserCommand.UpdateUser.Image
                : _module.ExistingUser.Image);
            
            updateUserResult.Response.UpdatedUser.Bio.Should().Be(updateBio
                ? _updateUserCommand.UpdateUser.Bio
                : _module.ExistingUser.Bio);
        }
        
        [Fact]
        public async Task GivenUpdateNothing_WhenUpdateUser_ThenFailsValidation()
        {
            //arrange
            _updateUserCommand.UpdateUser = new UpdateUserDTO();

            //act
            var updateUserResult = await _module.Mediator.Send(_updateUserCommand);

            //assert
            updateUserResult.Result.Should().Be(OperationResult.ValidationError);
            updateUserResult.Response.Should().BeNull();
        }

        [Fact]
        public async Task GivenUsernameAlreadyExists_WhenUpdateUser_ThenFailsValidation()
        {
            //arrange
            _updateUserCommand.UpdateUser.Username = _module.ExistingUser2.Username;

            //act
            var updateUserResult = await _module.Mediator.Send(_updateUserCommand);

            //assert
            updateUserResult.Result.Should().Be(OperationResult.ValidationError);
            updateUserResult.Response.Should().BeNull();
        }
        
        [Fact]
        public async Task GivenEmailAlreadyExists_WhenUpdateUser_ThenFailsValidation()
        {
            //arrange
            _updateUserCommand.UpdateUser.Email = _module.ExistingUser2.Email;

            //act
            var updateUserResult = await _module.Mediator.Send(_updateUserCommand);

            //assert
            updateUserResult.Result.Should().Be(OperationResult.ValidationError);
            updateUserResult.Response.Should().BeNull();
        }
        
        [Fact]
        public async Task GivenNonExistentUser_WhenUpdateUser_ThenFailsValidation()
        {
            //arrange
            _module.WithRandomUserContext();

            //act
            var updateUserResult = await _module.Mediator.Send(_updateUserCommand);

            //assert
            updateUserResult.Result.Should().Be(OperationResult.ValidationError);
            updateUserResult.Response.Should().BeNull();
        }
    }
}