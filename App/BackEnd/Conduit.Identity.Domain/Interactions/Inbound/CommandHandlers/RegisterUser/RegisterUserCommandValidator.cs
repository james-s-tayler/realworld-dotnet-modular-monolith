using System.Threading;
using System.Threading.Tasks;
using Conduit.Identity.Domain.Contracts.RegisterUser;
using Conduit.Identity.Domain.Interactions.Outbound.Repositories;
using FluentValidation;

namespace Conduit.Identity.Domain.Interactions.Inbound.CommandHandlers.RegisterUser
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        private readonly IUserRepository _userRepository;
        
        public RegisterUserCommandValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            
            RuleFor(command => command.NewUser.Username)
                .MustAsync(UsernameNotAlreadyInUse)
                .WithMessage(_ => "Username is already in use");
            RuleFor(command => command.NewUser.Email)
                .MustAsync(EmailNotAlreadyInUse)
                .WithMessage(_ => "Email is already in use");
        }
        
        private async Task<bool> UsernameNotAlreadyInUse(string username, CancellationToken cancellation)
        {
            return !await _userRepository.ExistsByUsername(username);
        }
        
        private async Task<bool> EmailNotAlreadyInUse(string email, CancellationToken cancellation)
        {
            return !await _userRepository.ExistsByEmail(email);
        }
    }
}