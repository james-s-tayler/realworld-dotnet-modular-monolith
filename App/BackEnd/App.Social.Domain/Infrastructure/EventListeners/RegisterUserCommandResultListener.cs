using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using App.Core.Context;
using App.Social.Domain.Infrastructure.Mappers;
using App.Social.Domain.Infrastructure.Repositories;
using App.Users.Domain.Contracts.Operations.Commands.RegisterUser;
using JetBrains.Annotations;
using MediatR;

namespace App.Social.Domain.Infrastructure.EventListeners
{
    internal class RegisterUserCommandResultListener : INotificationHandler<RegisterUserCommandResult>
    {
        private readonly IUserRepository _userRepository;

        public RegisterUserCommandResultListener([NotNull] IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Handle(RegisterUserCommandResult registerUserEvent, CancellationToken cancellationToken)
        {
            var userId = await _userRepository.Create(registerUserEvent.RegisteredUser.ToUser());
            await _userRepository.FollowSelf(userId);
        }
    }
}