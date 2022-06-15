using System.Threading;
using System.Threading.Tasks;
using Conduit.Core.PipelineBehaviors.OperationResponse;
using Conduit.Social.Domain.Contracts.Commands.UnfollowUser;
using Conduit.Social.Domain.Infrastructure.Mappers;
using Conduit.Social.Domain.Infrastructure.Repositories;
using JetBrains.Annotations;
using MediatR;

namespace Conduit.Social.Domain.Operations.Commands.UnfollowUser
{
    internal class UnfollowUserCommandHandler : IRequestHandler<UnfollowUserCommand, OperationResponse<UnfollowUserCommandResult>>
    {
        private readonly IUserRepository _userRepository;

        public UnfollowUserCommandHandler([NotNull] IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<OperationResponse<UnfollowUserCommandResult>> Handle(UnfollowUserCommand unfollowUserCommand, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByUsername(unfollowUserCommand.Username);
            await _userRepository.UnfollowUser(user.Id);

            return new OperationResponse<UnfollowUserCommandResult>(new UnfollowUserCommandResult
            {
                UnfollowedProfile = user.ToProfileDTO(false)
            });
        }
    }
}