using System.Threading;
using System.Threading.Tasks;
using App.Feed.Domain.Entities;
using App.Feed.Domain.Infrastructure.Repositories;
using App.Users.Domain.Contracts.Operations.Commands.UnfollowUser;
using JetBrains.Annotations;
using MediatR;

namespace App.Feed.Domain.Infrastructure.EventListeners
{
    internal class UnfollowUserCommandResultListener : INotificationHandler<UnfollowUserCommandResult>
    {
        private readonly IFollowRepository _followRepository;

        public UnfollowUserCommandResultListener([NotNull] IFollowRepository followRepository)
        {
            _followRepository = followRepository;
        }

        public async Task Handle(UnfollowUserCommandResult followUserEvent, CancellationToken cancellationToken)
        {
            var follow = new FollowEntity
            {
                UserId = followUserEvent.UserId,
                FollowingUserId = followUserEvent.FollowingUserId
            };
            await _followRepository.Unfollow(follow);
        }
    }
}