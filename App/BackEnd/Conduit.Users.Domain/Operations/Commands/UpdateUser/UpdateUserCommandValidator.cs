using System;
using System.Threading;
using System.Threading.Tasks;
using Conduit.Core.Context;
using Conduit.Identity.Domain.Contracts.Commands.UpdateUser;
using Conduit.Identity.Domain.Infrastructure.Repositories;
using FluentValidation;

namespace Conduit.Identity.Domain.Operations.Commands.UpdateUser
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserContext _userContext;
        public UpdateUserCommandValidator(IUserRepository userRepository,
            IUserContext userContext)
        {
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));

            RuleFor(command => command.UpdateUser)
                .Must(ContainUpdate)
                .WithMessage("At least one property must be updated.");
            RuleFor(query => query)
                .MustAsync(UserMustExist)
                .WithMessage($"User {_userContext.UserId} was not found.");
            RuleFor(command => command.UpdateUser.Username)
                .MustAsync(UsernameNotAlreadyInUse)
                .WithMessage(_ => "Username is already in use")
                .When(command => command.UpdateUser.Username != null);
            RuleFor(command => command.UpdateUser.Email)
                .MustAsync(EmailNotAlreadyInUse)
                .WithMessage(_ => "Email is already in use")
                .When(command => command.UpdateUser.Email != null);;
        }
        
        //this feels not very future proof?
        private bool ContainUpdate(UpdateUserDTO updateUser)
        {
            return updateUser.Bio != null ||
                   updateUser.Email != null ||
                   updateUser.Image != null ||
                   updateUser.Username != null;
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