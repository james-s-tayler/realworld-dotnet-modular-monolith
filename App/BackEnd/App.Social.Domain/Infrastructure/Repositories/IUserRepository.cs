using System.Threading.Tasks;
using App.Core.DataAccess;
using Application.Social.Domain.Entities;

namespace Application.Social.Domain.Infrastructure.Repositories
{
    internal interface IUserRepository : ICrudRepository<UserEntity, int>
    {
        Task<UserEntity> GetByUsername(string username);
        Task<bool> ExistsByUsername(string username);
        Task<bool> IsFollowing(int followUserId);
        Task FollowUser(int followUserId);
        Task UnfollowUser(int followUserId);
    }
}