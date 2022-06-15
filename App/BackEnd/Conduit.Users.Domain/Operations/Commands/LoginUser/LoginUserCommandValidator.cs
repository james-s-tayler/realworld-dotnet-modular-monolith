using System.Threading;
using System.Threading.Tasks;
using Application.Users.Domain.Contracts.Commands.LoginUser;
using Conduit.Users.Domain.Infrastructure.Repositories;
using FluentValidation;
using JetBrains.Annotations;

namespace Conduit.Users.Domain.Operations.Commands.LoginUser
{
    internal class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
    {
        private readonly IUserRepository _userRepository;
        
        public LoginUserCommandValidator([NotNull] IUserRepository userRepository)
        {
            _userRepository = userRepository;

            RuleFor(command => command.UserCredentials.Email)
                .MustAsync(UserMustExist)
                .WithMessage(_ => "The email address you entered isn't connected to an account.");
        }
        
        private async Task<bool> UserMustExist(string email, CancellationToken cancellation)
        {
            return await _userRepository.ExistsByEmail(email);
        }
    }
}