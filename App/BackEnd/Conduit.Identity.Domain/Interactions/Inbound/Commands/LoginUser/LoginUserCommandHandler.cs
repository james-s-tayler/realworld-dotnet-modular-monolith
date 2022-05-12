using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Conduit.Core.Validation;
using Conduit.Identity.Domain.Contracts.LoginUser;
using Conduit.Identity.Domain.Entities;
using Conduit.Identity.Domain.Interactions.Inbound.Services;
using Conduit.Identity.Domain.Interactions.Outbound.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Conduit.Identity.Domain.Interactions.Inbound.Commands.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, OperationResponse<LoginUserResult>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IAuthTokenService _authTokenService;

        public LoginUserCommandHandler(IUserRepository userRepository, 
            IPasswordHasher<User> passwordHasher, 
            IAuthTokenService authTokenService)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            _authTokenService = authTokenService ?? throw new ArgumentNullException(nameof(authTokenService));
        }
        
        public async Task<OperationResponse<LoginUserResult>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmail(request.UserCredentials.Email);
            if (user == null) 
                throw new ArgumentNullException(nameof(user));
            
            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, request.UserCredentials.Password);

            if (result == PasswordVerificationResult.Failed)
                return new OperationResponse<LoginUserResult>(LoginUserResult.FailedLoginResult());
            
            var token = await _authTokenService.GenerateAuthToken(user);
            
            return new OperationResponse<LoginUserResult>(LoginUserResult.SuccessfulLoginResult(new LoggedInUserDTO
            {
                Email = user.Email,
                Username = user.Username,
                Token = token
            }));
        }
    }
}