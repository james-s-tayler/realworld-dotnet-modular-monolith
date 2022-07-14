using System.Threading;
using System.Threading.Tasks;
using App.Core.PipelineBehaviors.OperationResponse;
using App.Social.Domain.Contracts.Operations.Commands.FollowUser;
using Application.Social.Domain.Infrastructure.Mappers;
using Application.Social.Domain.Infrastructure.Repositories;
using JetBrains.Annotations;
using MediatR;

namespace Application.Social.Domain.Operations.Commands.FollowUser
{
    internal class FollowUserCommandHandler : IRequestHandler<FollowUserCommand, OperationResponse<FollowUserCommandResult>>
    {
        private readonly IUserRepository _userRepository;

        public FollowUserCommandHandler([NotNull] IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<OperationResponse<FollowUserCommandResult>> Handle(FollowUserCommand followUserCommand, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByUsername(followUserCommand.Username);
            await _userRepository.FollowUser(user.Id);

            return new OperationResponse<FollowUserCommandResult>(new FollowUserCommandResult
            {
                FollowedProfile = user.ToProfileDTO(true)
            });
        }
    }
}