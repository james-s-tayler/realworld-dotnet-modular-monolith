using System.Threading;
using System.Threading.Tasks;
using Application.Core.PipelineBehaviors.OperationResponse;
using Application.Social.Domain.Contracts.Commands.UnfollowUser;
using Application.Social.Domain.Infrastructure.Mappers;
using Application.Social.Domain.Infrastructure.Repositories;
using JetBrains.Annotations;
using MediatR;

namespace Application.Social.Domain.Operations.Commands.UnfollowUser
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