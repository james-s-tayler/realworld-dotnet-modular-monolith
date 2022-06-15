using System.Threading.Tasks;
using Application.Core.DataAccess;
using Conduit.Users.Domain.Entities;

namespace Conduit.Users.Domain.Infrastructure.Repositories
{
    internal interface IUserRepository : ICrudRepository<User, int>
    {
        Task<User> GetByEmail(string email);
        Task<User> GetByUsername(string username);
        Task<bool> ExistsByUsername(string username);
        Task<bool> ExistsByEmail(string email);
    }
}