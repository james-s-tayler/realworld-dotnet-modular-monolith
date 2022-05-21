using System.Threading;
using System.Threading.Tasks;
using Conduit.Core.PipelineBehaviors;
using Conduit.Identity.Domain.Contracts.Commands.RegisterUser;
using Conduit.Identity.Domain.Entities;
using Conduit.Identity.Domain.Infrastructure.Mappers;
using Conduit.Identity.Domain.Infrastructure.Repositories;
using Conduit.Identity.Domain.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Conduit.Identity.Domain.Operations.Commands.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, OperationResponse<RegisterUserResult>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IAuthTokenService _authTokenService;

        public RegisterUserCommandHandler(IUserRepository userRepository,
            IPasswordHasher<User> passwordHasher, 
            IAuthTokenService authTokenService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _authTokenService = authTokenService;
        }

        public async Task<OperationResponse<RegisterUserResult>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var newUser = new User
            {
                Username = request.NewUser.Username,
                Email = request.NewUser.Email
            };

            newUser.Password = _passwordHasher.HashPassword(newUser, request.NewUser.Password);
            
            var userId = await _userRepository.Create(newUser);
            var user = await _userRepository.GetById(userId); 
            var token = await _authTokenService.GenerateAuthToken(user);
            
            return new OperationResponse<RegisterUserResult>(new RegisterUserResult
            {
                RegisteredUser = user.ToUserDTO(token),
                UserId = userId
            });
        }
    }
}