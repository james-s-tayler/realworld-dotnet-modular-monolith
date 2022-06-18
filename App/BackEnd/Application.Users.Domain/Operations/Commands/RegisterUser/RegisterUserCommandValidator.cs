using System.Threading;
using System.Threading.Tasks;
using Application.Users.Domain.Configuration;
using Application.Users.Domain.Contracts.Operations.Commands.RegisterUser;
using Application.Users.Domain.Infrastructure.Repositories;
using FluentValidation;
using JetBrains.Annotations;

namespace Application.Users.Domain.Operations.Commands.RegisterUser
{
    internal class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        private readonly IUserRepository _userRepository;
        
        public RegisterUserCommandValidator([NotNull] IUserRepository userRepository)
        {
            _userRepository = userRepository;

            RuleFor(command => command.NewUser.Username)
                .NotNull()
                .NotEmpty()
                .MaximumLength(Constants.UsernameMaxLength);
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
                .MinimumLength(10);
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