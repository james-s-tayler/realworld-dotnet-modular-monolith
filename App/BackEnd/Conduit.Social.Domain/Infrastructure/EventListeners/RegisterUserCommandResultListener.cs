using System.Threading;
using System.Threading.Tasks;
using Conduit.Social.Domain.Entities;
using Conduit.Social.Domain.Infrastructure.Mappers;
using Conduit.Social.Domain.Infrastructure.Repositories;
using Conduit.Users.Domain.Contracts.Commands.RegisterUser;
using JetBrains.Annotations;
using MediatR;

namespace Conduit.Social.Domain.Infrastructure.EventListeners
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
            await _userRepository.Create(registerUserEvent.RegisteredUser.ToUser());
        }
    }
}