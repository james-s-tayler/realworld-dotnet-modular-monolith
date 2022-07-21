using System.Threading;
using System.Threading.Tasks;
using App.Core.Context;
using App.Core.PipelineBehaviors.OperationResponse;
using App.Users.Domain.Contracts;
using App.Users.Domain.Contracts.Operations.Commands.FollowUser;
using App.Users.Domain.Infrastructure.Mappers;
using App.Users.Domain.Infrastructure.Repositories;
using JetBrains.Annotations;
using MediatR;

namespace App.Users.Domain.Operations.Commands.FollowUser
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
                UserId = _userContext.UserId,
                FollowingUserId = followUser.Id,
                FollowedProfile = UserMapper.ToProfileDTO(followUser, true)
            });
        }
    }
}