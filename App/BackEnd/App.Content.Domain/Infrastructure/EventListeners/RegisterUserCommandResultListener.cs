using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using App.Content.Domain.Infrastructure.Mappers;
using App.Content.Domain.Infrastructure.Repositories;
using App.Core.Context;
using App.Users.Domain.Contracts.Operations.Commands.RegisterUser;
using JetBrains.Annotations;
using MediatR;

namespace App.Content.Domain.Infrastructure.EventListeners
{
    internal class RegisterUserCommandResultListener : INotificationHandler<RegisterUserCommandResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRequestClaimsPrincipalProvider _claimsPrincipalProvider;

        public RegisterUserCommandResultListener([NotNull] IUserRepository userRepository, 
            [NotNull] IRequestClaimsPrincipalProvider claimsPrincipalProvider)
        {
            _userRepository = userRepository;
            _claimsPrincipalProvider = claimsPrincipalProvider;
        }

        public async Task Handle(RegisterUserCommandResult registerUserEvent, CancellationToken cancellationToken)
        {
            var userId = await _userRepository.Create(registerUserEvent.RegisteredUser.ToUser());

            //this will be null inside a unit test, but in the case of a unit test IUserContext will be ITestContext anyway
            
                var identity = new ClaimsIdentity(new Claim[]
                {
                    new Claim("user_id", userId.ToString()),
                    new Claim("username", registerUserEvent.RegisteredUser.Username),
                    new Claim("email", registerUserEvent.RegisteredUser.Email)
                }, "Basic");
          
                var principal = new ClaimsPrincipal(identity);
                _claimsPrincipalProvider.SetClaimsPrincipal(principal);   
            
        }
    }
}