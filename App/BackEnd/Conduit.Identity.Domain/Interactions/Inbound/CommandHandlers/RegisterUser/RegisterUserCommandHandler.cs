using System.Threading;
using System.Threading.Tasks;
using Conduit.Core.Validation;
using Conduit.Identity.Domain.Contracts.RegisterUser;
using Conduit.Identity.Domain.Entities;
using Conduit.Identity.Domain.Interactions.Outbound.Repositories;
using MediatR;

namespace Conduit.Identity.Domain.Interactions.Inbound.CommandHandlers.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, ValidateableResponse<RegisterUserResult>>
    {
        private readonly IUserRepository _userRepository;

        public RegisterUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ValidateableResponse<RegisterUserResult>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var newUser = new User
            {
                Username = request.NewUser.Username,
                Email = request.NewUser.Email,
                Password = request.NewUser.Password
            };
            
            var userId = await _userRepository.Create(newUser);
            
            return new ValidateableResponse<RegisterUserResult>(new RegisterUserResult
            {
                UserId = userId
            });
        }
    }
}