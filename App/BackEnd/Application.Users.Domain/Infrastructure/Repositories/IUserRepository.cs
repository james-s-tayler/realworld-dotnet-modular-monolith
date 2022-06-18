using System.Threading.Tasks;
using Application.Core.DataAccess;
using Application.Users.Domain.Entities;

namespace Application.Users.Domain.Infrastructure.Repositories
{
    internal interface IUserRepository : ICrudRepository<User, int>
    {
        Task<User> GetByEmail(string email);
        Task<User> GetByUsername(string username);
        Task<bool> ExistsByUsername(string username);
        Task<bool> ExistsByEmail(string email);
    }
}