using System.Threading;
using System.Threading.Tasks;
using App.Core.Context;
using App.Core.PipelineBehaviors.OperationResponse;
using App.Social.Domain.Contracts.Operations.Commands.UnfollowUser;
using App.Social.Domain.Infrastructure.Mappers;
using App.Social.Domain.Infrastructure.Repositories;
using JetBrains.Annotations;
using MediatR;

namespace App.Social.Domain.Operations.Commands.UnfollowUser
{
    internal class UnfollowUserCommandHandler : IRequestHandler<UnfollowUserCommand, OperationResponse<UnfollowUserCommandResult>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserContext _userContext;

        public UnfollowUserCommandHandler([NotNull] IUserRepository userRepository, 
            [NotNull] IUserContext userContext)
        {
            _userRepository = userRepository;
            _userContext = userContext;
        }

        public async Task<OperationResponse<UnfollowUserCommandResult>> Handle(UnfollowUserCommand unfollowUserCommand, CancellationToken cancellationToken)
        {
            var unfollowUserId = await _userRepository.GetByUsername(unfollowUserCommand.Username);
            await _userRepository.UnfollowUser(_userContext.UserId, unfollowUserId.Id);

            return new OperationResponse<UnfollowUserCommandResult>(new UnfollowUserCommandResult
            {
                UnfollowedProfile = unfollowUserId.ToProfileDTO(false)
            });
        }
    }
}