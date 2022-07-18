using System.Threading;
using System.Threading.Tasks;
using App.Content.Domain.Infrastructure.Mappers;
using App.Content.Domain.Infrastructure.Repositories;
using App.Users.Domain.Contracts.Operations.Commands.RegisterUser;
using JetBrains.Annotations;
using MediatR;

namespace App.Content.Domain.Infrastructure.EventListeners
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
            _ = await _userRepository.Create(registerUserEvent.RegisteredUser.ToUser());
        }
    }
}