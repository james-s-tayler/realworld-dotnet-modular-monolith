using System.Threading.Tasks;
using Application.Core.DataAccess;
using Conduit.Social.Domain.Entities;

namespace Conduit.Social.Domain.Infrastructure.Repositories
{
    internal interface IUserRepository : ICrudRepository<User, int>
    {
        Task<User> GetByUsername(string username);
        Task<bool> ExistsByUsername(string username);
        Task<bool> IsFollowing(int followUserId);
        Task FollowUser(int followUserId);
        Task UnfollowUser(int followUserId);
    }
}