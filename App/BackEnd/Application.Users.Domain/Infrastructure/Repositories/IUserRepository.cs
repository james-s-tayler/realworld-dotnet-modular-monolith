using System.Threading.Tasks;
using Application.Core.DataAccess;
using Application.Users.Domain.Entities;

namespace Application.Users.Domain.Infrastructure.Repositories
{
    internal interface IUserRepository : ICrudRepository<UserEntity, int>
    {
        Task<UserEntity> GetByEmail(string email);
        Task<UserEntity> GetByUsername(string username);
        Task<bool> ExistsByUsername(string username);
        Task<bool> ExistsByEmail(string email);
    }
}