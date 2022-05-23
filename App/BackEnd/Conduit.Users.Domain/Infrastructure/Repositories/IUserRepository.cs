using System.Threading.Tasks;
using Conduit.Core.DataAccess;
using Conduit.Users.Domain.Entities;

namespace Conduit.Users.Domain.Infrastructure.Repositories
{
    public interface IUserRepository : ICrudRepository<User, int>
    {
        Task<User> GetByEmail(string email);
        Task<bool> ExistsByUsername(string username);
        Task<bool> ExistsByEmail(string email);
    }
}