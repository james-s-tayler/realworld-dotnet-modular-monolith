using System.Threading;
using System.Threading.Tasks;
using App.Social.Domain.Infrastructure.Mappers;
using App.Social.Domain.Infrastructure.Repositories;
using App.Users.Domain.Contracts.Operations.Commands.UpdateUser;
using JetBrains.Annotations;
using MediatR;

namespace App.Social.Domain.Infrastructure.EventListeners
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