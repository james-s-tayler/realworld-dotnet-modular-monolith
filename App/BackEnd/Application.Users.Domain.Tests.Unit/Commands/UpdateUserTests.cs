using System.Threading.Tasks;
using Application.Core.PipelineBehaviors.OperationResponse;
using Application.Core.Testing;
using Application.Users.Domain.Contracts.Commands.UpdateUser;
using Application.Users.Domain.Tests.Unit.Setup;
using AutoFixture;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Application.Users.Domain.Tests.Unit.Commands
{
    [Collection(nameof(UsersModuleTestCollection))]
    public class UpdateUserTests : TestBase
    {
        private readonly UsersModuleSetupFixture _usersModule;
        private readonly UpdateUserCommand _updateUserCommand;

        public UpdateUserTests(UsersModuleSetupFixture usersModule, ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _usersModule = usersModule;
            _usersModule.WithAuthenticatedUserContext();
            _usersModule.WithUserRepoContainingDefaultUsers().GetAwaiter().GetResult();
            _updateUserCommand = new UpdateUserCommand
            {
                UpdateUser = new UpdateUserDTO
                {
                    Bio = _usersModule.AutoFixture.Create<string>(),
                    Email = _usersModule.AutoFixture.Create<string>(),
                    Image = _usersModule.AutoFixture.Create<string>(),
                    Username = _usersModule.AutoFixture.Create<string>()
                }
            };

            _usersModule.WithUserRepoContainingDefaultUsers().GetAwaiter().GetResult();
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
            var updateUserResult = await _usersModule.Mediator.Send(_updateUserCommand);

            //assert
            updateUserResult.Result.Should().Be(OperationResult.Success);
            updateUserResult.Response.Should().NotBeNull();
            updateUserResult.Response.UpdatedUser.Id.Should().Be(_usersModule.ExistingUser.Id);

            updateUserResult.Response.UpdatedUser.Username.Should().Be(updateUsername
                ? _updateUserCommand.UpdateUser.Username
                : _usersModule.ExistingUser.Username);
            
            updateUserResult.Response.UpdatedUser.Email.Should().Be(updateEmail
                ? _updateUserCommand.UpdateUser.Email
                : _usersModule.ExistingUser.Email);
            
            updateUserResult.Response.UpdatedUser.Image.Should().Be(updateImage
                ? _updateUserCommand.UpdateUser.Image
                : _usersModule.ExistingUser.Image);
            
            updateUserResult.Response.UpdatedUser.Bio.Should().Be(updateBio
                ? _updateUserCommand.UpdateUser.Bio
                : _usersModule.ExistingUser.Bio);
        }
        
        [Fact]
        public async Task GivenUpdateEmailWithExistingValue_WhenUpdateUser_ThenSucceeds()
        {
            //arrange
            _updateUserCommand.UpdateUser = new UpdateUserDTO {Email = _usersModule.ExistingUser.Email};

            //act
            var updateUserResult = await _usersModule.Mediator.Send(_updateUserCommand);

            //assert
            updateUserResult.Result.Should().Be(OperationResult.Success);
            updateUserResult.Response.Should().NotBeNull();
            updateUserResult.Response.UpdatedUser.Id.Should().Be(_usersModule.ExistingUser.Id);

            updateUserResult.Response.UpdatedUser.Email.Should().Be(_usersModule.ExistingUser.Email);
        }
       
        [Fact]
        public async Task GivenUpdateUsernameWithExistingValue_WhenUpdateUser_ThenSucceeds()
        {
            //arrange
            _updateUserCommand.UpdateUser = new UpdateUserDTO {Email = _usersModule.ExistingUser.Username};

            //act
            var updateUserResult = await _usersModule.Mediator.Send(_updateUserCommand);

            //assert
            updateUserResult.Result.Should().Be(OperationResult.Success);
            updateUserResult.Response.Should().NotBeNull();
            updateUserResult.Response.UpdatedUser.Id.Should().Be(_usersModule.ExistingUser.Id);

            updateUserResult.Response.UpdatedUser.Username.Should().Be(_usersModule.ExistingUser.Username);
        }
        
        [Fact]
        public async Task GivenUpdateNothing_WhenUpdateUser_ThenFailsValidation()
        {
            //arrange
            _updateUserCommand.UpdateUser = new UpdateUserDTO();

            //act
            var updateUserResult = await _usersModule.Mediator.Send(_updateUserCommand);

            //assert
            updateUserResult.Result.Should().Be(OperationResult.ValidationError);
            updateUserResult.Response.Should().BeNull();
        }

        [Fact]
        public async Task GivenUsernameAlreadyExists_WhenUpdateUser_ThenFailsValidation()
        {
            //arrange
            _updateUserCommand.UpdateUser.Username = _usersModule.ExistingUser2.Username;

            //act
            var updateUserResult = await _usersModule.Mediator.Send(_updateUserCommand);

            //assert
            updateUserResult.Result.Should().Be(OperationResult.ValidationError);
            updateUserResult.Response.Should().BeNull();
        }
        
        [Fact]
        public async Task GivenEmailAlreadyExists_WhenUpdateUser_ThenFailsValidation()
        {
            //arrange
            _updateUserCommand.UpdateUser.Email = _usersModule.ExistingUser2.Email;

            //act
            var updateUserResult = await _usersModule.Mediator.Send(_updateUserCommand);

            //assert
            updateUserResult.Result.Should().Be(OperationResult.ValidationError);
            updateUserResult.Response.Should().BeNull();
        }
        
        [Fact]
        public async Task GivenNonExistentUser_WhenUpdateUser_ThenFailsValidation()
        {
            //arrange
            _usersModule.WithRandomUserContext();

            //act
            var updateUserResult = await _usersModule.Mediator.Send(_updateUserCommand);

            //assert
            updateUserResult.Result.Should().Be(OperationResult.ValidationError);
            updateUserResult.Response.Should().BeNull();
        }
    }
}