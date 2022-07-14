using System.Threading.Tasks;
using App.Core.DataAccess;
using App.Users.Domain.Entities;

namespace App.Users.Domain.Infrastructure.Repositories
{
    internal interface IUserRepository : ICrudRepository<UserEntity, int>
    {
        Task<UserEntity> GetByEmail(string email);
        Task<UserEntity> GetByUsername(string username);
        Task<bool> ExistsByUsername(string username);
        Task<bool> ExistsByEmail(string email);
    }
}