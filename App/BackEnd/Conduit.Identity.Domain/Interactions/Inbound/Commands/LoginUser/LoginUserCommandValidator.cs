using System.Threading;
using System.Threading.Tasks;
using Conduit.Identity.Domain.Contracts.LoginUser;
using Conduit.Identity.Domain.Interactions.Outbound.Repositories;
using FluentValidation;

namespace Conduit.Identity.Domain.Interactions.Inbound.Commands.LoginUser
{
    public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
    {
        private readonly IUserRepository _userRepository;
        
        public LoginUserCommandValidator(IUserRepository userRepository)
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