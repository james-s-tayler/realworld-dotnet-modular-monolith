using System;
using System.Threading;
using System.Threading.Tasks;
using Conduit.Core.Validation;
using Conduit.Identity.Domain.Contracts.LoginUser;
using Conduit.Identity.Domain.Entities;
using Conduit.Identity.Domain.Interactions.Outbound.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Conduit.Identity.Domain.Interactions.Inbound.Commands.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, OperationResponse<LoginUserResult>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;

        public LoginUserCommandHandler(IUserRepository userRepository, 
            IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        }
        
        public async Task<OperationResponse<LoginUserResult>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmail(request.UserCredentials.Email);
            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, request.UserCredentials.Password);

            if (result == PasswordVerificationResult.Failed)
                return new OperationResponse<LoginUserResult>(LoginUserResult.FailedLoginResult());
            
            //generate token
            var token = "jwt";
            
            return new OperationResponse<LoginUserResult>(LoginUserResult.SuccessfulLoginResult(new LoggedInUserDTO
            {
                Email = user.Email,
                Username = user.Username,
                Token = token
            }));
        }
    }
}