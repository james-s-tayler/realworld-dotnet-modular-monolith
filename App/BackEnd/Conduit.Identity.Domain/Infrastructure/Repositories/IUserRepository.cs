using System.Threading.Tasks;
using Conduit.Core.Validation.DataAccess;
using Conduit.Identity.Domain.Entities;

namespace Conduit.Identity.Domain.Infrastructure.Repositories
{
    public interface IUserRepository : ICrudRepository<User, int>
    {
        Task<User> GetByEmail(string email);
        Task<bool> ExistsByUsername(string username);
        Task<bool> ExistsByEmail(string email);
    }
}