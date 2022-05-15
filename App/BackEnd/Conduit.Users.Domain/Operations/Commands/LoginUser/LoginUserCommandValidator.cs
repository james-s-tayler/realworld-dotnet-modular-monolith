using System;
using System.Threading;
using System.Threading.Tasks;
using Conduit.Identity.Domain.Contracts.Commands.LoginUser;
using Conduit.Identity.Domain.Infrastructure.Repositories;
using FluentValidation;

namespace Conduit.Identity.Domain.Operations.Commands.LoginUser
{
    public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
    {
        private readonly IUserRepository _userRepository;
        
        public LoginUserCommandValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));

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