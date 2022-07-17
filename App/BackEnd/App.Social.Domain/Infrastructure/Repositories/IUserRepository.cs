using System.Threading.Tasks;
using App.Core.DataAccess;
using App.Social.Domain.Entities;

namespace App.Social.Domain.Infrastructure.Repositories
{
    internal interface IUserRepository : ICrudRepository<UserEntity, int>
    {
        Task<UserEntity> GetByUsername(string username);
        Task<bool> ExistsByUsername(string username);
        Task<bool> IsFollowing(int userId, int followUserId);
        Task FollowSelf(int userId);
        Task FollowUser(int userId, int followUserId);
        Task UnfollowUser(int userId, int followUserId);
    }
}