using System.Threading;
using System.Threading.Tasks;
using Application.Core.Context;
using Conduit.Users.Domain.Configuration;
using Conduit.Users.Domain.Contracts.Commands.UpdateUser;
using Conduit.Users.Domain.Infrastructure.Repositories;
using FluentValidation;
using JetBrains.Annotations;

namespace Conduit.Users.Domain.Operations.Commands.UpdateUser
{
    internal class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserContext _userContext;
        public UpdateUserCommandValidator([NotNull] IUserRepository userRepository,
            [NotNull] IUserContext userContext)
        {
            _userContext = userContext;
            _userRepository = userRepository;

            RuleFor(command => command.UpdateUser)
                .Must(ContainUpdate)
                .WithMessage("At least one property must be updated.");
            RuleFor(command => command.UpdateUser.Username)
                .MaximumLength(Constants.UsernameMaxLength)
                .When(command => !string.IsNullOrEmpty(command.UpdateUser.Username));
            RuleFor(command => command.UpdateUser.Image)
                .MaximumLength(Constants.ImageUriMaxLength)
                .When(command => !string.IsNullOrEmpty(command.UpdateUser.Image));
            RuleFor(command => command.UpdateUser.Bio)
                .MaximumLength(Constants.BioMaxLength)
                .When(command => !string.IsNullOrEmpty(command.UpdateUser.Bio));
            RuleFor(command => command)
                .MustAsync(UserMustExist)
                .WithMessage($"User {_userContext.UserId} was not found.");
            RuleFor(command => command.UpdateUser.Username)
                .MustAsync(UsernameNotAlreadyInUse)
                .WithMessage(_ => "Username is already in use")
                .WhenAsync(ShouldValidateUsername);
            RuleFor(command => command.UpdateUser.Email)
                .MustAsync(EmailNotAlreadyInUse)
                .WithMessage(_ => "Email is already in use")
                .WhenAsync(ShouldValidateEmail);
        }
        
        //this feels not very future proof?
        private bool ContainUpdate(UpdateUserDTO updateUser)
        {
            return updateUser.Bio != null ||
                   updateUser.Email != null ||
                   updateUser.Image != null ||
                   updateUser.Username != null;
        }

        private async Task<bool> ShouldValidateEmail(UpdateUserCommand command, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetById(_userContext.UserId);
            
            if (user != null && user.Email == command.UpdateUser.Email)
                return false;
            
            return command.UpdateUser.Email != null;
        }
        
        private async Task<bool> ShouldValidateUsername(UpdateUserCommand command, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetById(_userContext.UserId);
            
            if (user != null && user.Username == command.UpdateUser.Username)
                return false;
            
            return command.UpdateUser.Username != null;
        }
        
        //copy pasta from RegisterUser - find way to re-use validators (some sort of ValidationRequirement class?)
        private async Task<bool> UsernameNotAlreadyInUse(string username, CancellationToken cancellation)
        {
            return !await _userRepository.ExistsByUsername(username);
        }
        
        private async Task<bool> EmailNotAlreadyInUse(string email, CancellationToken cancellation)
        {
            return !await _userRepository.ExistsByEmail(email);
        }
        
        //copy pasta from GetCurrentUserQueryValidator
        private async Task<bool> UserMustExist(UpdateUserCommand command, CancellationToken cancellationToken)
        {
            return await _userRepository.Exists(_userContext.UserId);
        }
    }
}