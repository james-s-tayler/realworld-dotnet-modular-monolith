using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using App.Content.Domain.Infrastructure.Mappers;
using App.Content.Domain.Infrastructure.Repositories;
using App.Users.Domain.Contracts.Operations.Commands.RegisterUser;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace App.Content.Domain.Infrastructure.EventListeners
{
    internal class RegisterUserCommandResultListener : INotificationHandler<RegisterUserCommandResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RegisterUserCommandResultListener([NotNull] IUserRepository userRepository,
            [NotNull] IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task Handle(RegisterUserCommandResult registerUserEvent, CancellationToken cancellationToken)
        {
            var userId = await _userRepository.Create(registerUserEvent.RegisteredUser.ToUser());

            //this will be null inside a unit test, but in the case of a unit test IUserContext will be ITestContext anyway
            if (_httpContextAccessor.HttpContext != null) 
            {
                var identity = new ClaimsIdentity(new Claim[]
                {
                    new Claim("user_id", userId.ToString()),
                    new Claim("username", registerUserEvent.RegisteredUser.Username),
                    new Claim("email", registerUserEvent.RegisteredUser.Email)
                });
          
                var principal = new ClaimsPrincipal(identity);

                _httpContextAccessor.HttpContext!.User = principal;   
            }
        }
    }
}