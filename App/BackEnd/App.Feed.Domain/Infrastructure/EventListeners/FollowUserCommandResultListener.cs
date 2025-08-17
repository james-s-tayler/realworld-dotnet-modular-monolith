using System.Threading;
using System.Threading.Tasks;
using App.Core.Context;
using App.Feed.Domain.Entities;
using App.Feed.Domain.Infrastructure.Repositories;
using App.Users.Domain.Contracts.Operations.Commands.FollowUser;
using JetBrains.Annotations;
using MediatR;

namespace App.Feed.Domain.Infrastructure.EventListeners
{
    internal class FollowUserCommandResultListener : INotificationHandler<FollowUserCommandResult>
    {
        private readonly IFollowRepository _followRepository;

        public FollowUserCommandResultListener([NotNull] IFollowRepository followRepository)
        {
            _followRepository = followRepository;
        }

        public async Task Handle(FollowUserCommandResult followUserEvent, CancellationToken cancellationToken)
        {
            if ( !await _followRepository.IsFollowing(followUserEvent.UserId,
                    followUserEvent.FollowingUserId) )
            {
                var follow = new FollowEntity
                {
                    UserId = followUserEvent.UserId,
                    FollowingUserId = followUserEvent.FollowingUserId
                };
                _ = await _followRepository.Follow(follow);
            }
        }
    }
}