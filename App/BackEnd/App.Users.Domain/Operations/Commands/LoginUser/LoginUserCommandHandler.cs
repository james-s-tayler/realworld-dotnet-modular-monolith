using System;
using System.Threading;
using System.Threading.Tasks;
using App.Core.PipelineBehaviors.OperationResponse;
using App.Users.Domain.Contracts.Operations.Commands.LoginUser;
using App.Users.Domain.Entities;
using App.Users.Domain.Infrastructure.Mappers;
using App.Users.Domain.Infrastructure.Repositories;
using App.Users.Domain.Infrastructure.Services;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace App.Users.Domain.Operations.Commands.LoginUser
{
    internal class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, OperationResponse<LoginUserCommandResult>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<UserEntity> _passwordHasher;
        private readonly IAuthTokenService _authTokenService;

        public LoginUserCommandHandler([NotNull] IUserRepository userRepository,
            [NotNull] IPasswordHasher<UserEntity> passwordHasher,
            [NotNull] IAuthTokenService authTokenService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _authTokenService = authTokenService;
        }

        public async Task<OperationResponse<LoginUserCommandResult>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmail(request.UserCredentials.Email);
            if ( user == null )
                return new OperationResponse<LoginUserCommandResult>(LoginUserCommandResult.FailedLoginResult());

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, request.UserCredentials.Password);

            if ( result == PasswordVerificationResult.Failed )
                return new OperationResponse<LoginUserCommandResult>(LoginUserCommandResult.FailedLoginResult());

            var token = await _authTokenService.GenerateAuthToken(user);

            return new OperationResponse<LoginUserCommandResult>(LoginUserCommandResult.SuccessfulLoginResult(user.ToUserDTO(token)));
        }
    }
}