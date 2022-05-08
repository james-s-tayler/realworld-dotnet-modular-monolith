using System.Threading;
using System.Threading.Tasks;
using Conduit.Core.Validation;
using Conduit.Identity.Domain.Contracts.RegisterUser;
using Conduit.Identity.Domain.Entities;
using Conduit.Identity.Domain.Interactions.Outbound.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Conduit.Identity.Domain.Interactions.Inbound.Commands.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, ValidateableResponse<RegisterUserResult>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;

        public RegisterUserCommandHandler(IUserRepository userRepository,
            IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<ValidateableResponse<RegisterUserResult>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var newUser = new User
            {
                Username = request.NewUser.Username,
                Email = request.NewUser.Email
            };

            newUser.Password = _passwordHasher.HashPassword(newUser, request.NewUser.Password);
            
            var userId = await _userRepository.Create(newUser);
            
            return new ValidateableResponse<RegisterUserResult>(new RegisterUserResult
            {
                UserId = userId
            });
        }
    }
}