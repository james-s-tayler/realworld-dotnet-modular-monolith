using System;
using System.Threading;
using System.Threading.Tasks;
using Conduit.Core.PipelineBehaviors;
using Conduit.Users.Domain.Contracts.Commands.LoginUser;
using Conduit.Users.Domain.Entities;
using Conduit.Users.Domain.Infrastructure.Mappers;
using Conduit.Users.Domain.Infrastructure.Repositories;
using Conduit.Users.Domain.Infrastructure.Services;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Conduit.Users.Domain.Operations.Commands.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, OperationResponse<LoginUserCommandResult>>
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
        
        public async Task<OperationResponse<LoginUserCommandResult>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmail(request.UserCredentials.Email);
            if (user == null) 
                throw new ArgumentNullException(nameof(user));
            
            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, request.UserCredentials.Password);

            if (result == PasswordVerificationResult.Failed)
                return new OperationResponse<LoginUserCommandResult>(LoginUserCommandResult.FailedLoginResult());
            
            var token = await _authTokenService.GenerateAuthToken(user);
            
            return new OperationResponse<LoginUserCommandResult>(LoginUserCommandResult.SuccessfulLoginResult(user.ToUserDTO(token)));
        }
    }
}