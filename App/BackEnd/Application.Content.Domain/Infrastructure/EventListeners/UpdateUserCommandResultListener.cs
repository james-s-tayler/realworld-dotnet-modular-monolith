using System.Threading;
using System.Threading.Tasks;
using Application.Content.Domain.Infrastructure.Mappers;
using Application.Content.Domain.Infrastructure.Repositories;
using Application.Users.Domain.Contracts.Operations.Commands.UpdateUser;
using JetBrains.Annotations;
using MediatR;

namespace Application.Content.Domain.Infrastructure.EventListeners
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