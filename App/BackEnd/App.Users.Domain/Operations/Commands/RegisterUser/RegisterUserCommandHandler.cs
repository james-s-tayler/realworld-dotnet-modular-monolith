using System.Threading;
using System.Threading.Tasks;
using App.Core.PipelineBehaviors.OperationResponse;
using App.Users.Domain.Contracts.Operations.Commands.RegisterUser;
using App.Users.Domain.Entities;
using App.Users.Domain.Infrastructure.Mappers;
using App.Users.Domain.Infrastructure.Repositories;
using App.Users.Domain.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace App.Users.Domain.Operations.Commands.RegisterUser
{
    internal class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, OperationResponse<RegisterUserCommandResult>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<UserEntity> _passwordHasher;
        private readonly IAuthTokenService _authTokenService;

        public RegisterUserCommandHandler(IUserRepository userRepository,
            IPasswordHasher<UserEntity> passwordHasher,
            IAuthTokenService authTokenService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _authTokenService = authTokenService;
        }

        public async Task<OperationResponse<RegisterUserCommandResult>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var newUser = new UserEntity
            {
                Username = request.NewUser.Username,
                Email = request.NewUser.Email
            };

            newUser.Password = _passwordHasher.HashPassword(newUser, request.NewUser.Password);

            var userId = await _userRepository.Create(newUser);
            var user = await _userRepository.GetById(userId);
            var token = await _authTokenService.GenerateAuthToken(user);

            return new OperationResponse<RegisterUserCommandResult>(new RegisterUserCommandResult
            {
                RegisteredUser = user.ToUserDTO(token)
            });
        }
    }
}