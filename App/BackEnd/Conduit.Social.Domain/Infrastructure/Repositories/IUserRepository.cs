using System.Threading.Tasks;
using Conduit.Core.DataAccess;
using Conduit.Social.Domain.Entities;

namespace Conduit.Social.Domain.Infrastructure.Repositories
{
    internal interface IUserRepository : ICrudRepository<User, int>
    {
        Task<User> GetByUsername(string username);
        Task<bool> ExistsByUsername(string username);
        Task<bool> IsFollowing(int userId, int followingUserId);
    }
}