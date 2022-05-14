using System;
using System.Threading;
using System.Threading.Tasks;
using Conduit.Core.PipelineBehaviors;
using Conduit.Identity.Domain.Contracts;
using Conduit.Identity.Domain.Contracts.Commands.LoginUser;
using Conduit.Identity.Domain.Entities;
using Conduit.Identity.Domain.Infrastructure.Repositories;
using Conduit.Identity.Domain.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Conduit.Identity.Domain.Operations.Commands.LoginUser
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
            
            //could make this result just return the token, then login action can call GetCurrentUserQuery after
            var token = await _authTokenService.GenerateAuthToken(user);
            
            return new OperationResponse<LoginUserResult>(LoginUserResult.SuccessfulLoginResult(new UserDTO
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username,
                Token = token
            }));
        }
    }
}