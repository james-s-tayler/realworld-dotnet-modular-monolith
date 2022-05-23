using System.Threading;
using System.Threading.Tasks;
using Conduit.Users.Domain.Contracts.Commands.RegisterUser;
using Conduit.Users.Domain.Infrastructure.Repositories;
using FluentValidation;
using JetBrains.Annotations;

namespace Conduit.Users.Domain.Operations.Commands.RegisterUser
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        private readonly IUserRepository _userRepository;
        
        public RegisterUserCommandValidator([NotNull] IUserRepository userRepository)
        {
            _userRepository = userRepository;

            RuleFor(command => command.NewUser.Username)
                .NotNull()
                .NotEmpty();
            RuleFor(command => command.NewUser.Username)
                .MustAsync(UsernameNotAlreadyInUse)
                .WithMessage(_ => "Username is already in use")
                .When(command => command.NewUser.Username != null);
            RuleFor(command => command.NewUser.Email)
                .NotNull()
                .NotEmpty();
            RuleFor(command => command.NewUser.Email)
                .MustAsync(EmailNotAlreadyInUse)
                .WithMessage(_ => "Email is already in use")
                .When(command => command.NewUser.Email != null);
            RuleFor(command => command.NewUser.Password)
                .NotNull()
                .NotEmpty()
                .MinimumLength(8);
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