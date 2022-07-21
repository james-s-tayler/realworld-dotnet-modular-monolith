using System.Threading.Tasks;
using App.Feed.Domain.Entities;

namespace App.Feed.Domain.Infrastructure.Repositories
{
    internal interface IFollowRepository
    {
        Task<bool> IsFollowing(int userId, int followingUserId);
        Task<FollowEntity> Follow(FollowEntity follow);
        Task Unfollow(FollowEntity follow);
    }
}