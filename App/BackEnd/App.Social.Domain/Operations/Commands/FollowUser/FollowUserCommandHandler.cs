using System.Threading;
using System.Threading.Tasks;
using App.Core.Context;
using App.Core.PipelineBehaviors.OperationResponse;
using App.Social.Domain.Contracts.Operations.Commands.FollowUser;
using App.Social.Domain.Infrastructure.Mappers;
using App.Social.Domain.Infrastructure.Repositories;
using JetBrains.Annotations;
using MediatR;

namespace App.Social.Domain.Operations.Commands.FollowUser
{
    internal class FollowUserCommandHandler : IRequestHandler<FollowUserCommand, OperationResponse<FollowUserCommandResult>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserContext _userContext;

        public FollowUserCommandHandler([NotNull] IUserRepository userRepository, 
            [NotNull] IUserContext userContext)
        {
            _userRepository = userRepository;
            _userContext = userContext;
        }

        public async Task<OperationResponse<FollowUserCommandResult>> Handle(FollowUserCommand followUserCommand, CancellationToken cancellationToken)
        {
            var followUser = await _userRepository.GetByUsername(followUserCommand.Username);
            await _userRepository.FollowUser(_userContext.UserId, followUser.Id);

            return new OperationResponse<FollowUserCommandResult>(new FollowUserCommandResult
            {
                FollowedProfile = followUser.ToProfileDTO(true)
            });
        }
    }
}