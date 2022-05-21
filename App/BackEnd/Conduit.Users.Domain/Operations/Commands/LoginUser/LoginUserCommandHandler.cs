using System;
using System.Threading;
using System.Threading.Tasks;
using Conduit.Core.PipelineBehaviors;
using Conduit.Identity.Domain.Contracts.Commands.LoginUser;
using Conduit.Identity.Domain.Entities;
using Conduit.Identity.Domain.Infrastructure.Mappers;
using Conduit.Identity.Domain.Infrastructure.Repositories;
using Conduit.Identity.Domain.Infrastructure.Services;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Conduit.Identity.Domain.Operations.Commands.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, OperationResponse<LoginUserResult>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IAuthTokenService _authTokenService;

        public LoginUserCommandHandler([NotNull] IUserRepository userRepository, 
            [NotNull] IPasswordHasher<User> passwordHasher, 
            [NotNull] IAuthTokenService authTokenService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _authTokenService = authTokenService;
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
            
            return new OperationResponse<LoginUserResult>(LoginUserResult.SuccessfulLoginResult(user.ToUserDTO(token)));
        }
    }
}