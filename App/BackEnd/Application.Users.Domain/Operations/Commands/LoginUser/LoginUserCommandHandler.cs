using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.PipelineBehaviors.OperationResponse;
using Application.Users.Domain.Contracts.Operations.Commands.LoginUser;
using Application.Users.Domain.Entities;
using Application.Users.Domain.Infrastructure.Mappers;
using Application.Users.Domain.Infrastructure.Repositories;
using Application.Users.Domain.Infrastructure.Services;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Users.Domain.Operations.Commands.LoginUser
{
    internal class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, OperationResponse<LoginUserCommandResult>>
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