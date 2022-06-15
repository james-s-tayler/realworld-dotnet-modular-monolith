using System.Threading;
using System.Threading.Tasks;
using Application.Users.Domain.Contracts.Commands.UpdateUser;
using Conduit.Social.Domain.Infrastructure.Mappers;
using Conduit.Social.Domain.Infrastructure.Repositories;
using JetBrains.Annotations;
using MediatR;

namespace Conduit.Social.Domain.Infrastructure.EventListeners
{
    internal class UpdateUserCommandResultListener : INotificationHandler<UpdateUserCommandResult>
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserCommandResultListener([NotNull] IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Handle(UpdateUserCommandResult updateUserEvent, CancellationToken cancellationToken)
        {
            await _userRepository.Update(updateUserEvent.UpdatedUser.ToUser());
        }
    }
}