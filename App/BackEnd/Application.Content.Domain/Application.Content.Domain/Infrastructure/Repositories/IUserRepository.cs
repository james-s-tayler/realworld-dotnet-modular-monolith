using System.Threading.Tasks;
using Application.Content.Domain.Entities;
using Application.Core.DataAccess;

namespace Application.Content.Domain.Infrastructure.Repositories
{
    internal interface IUserRepository : ICrudRepository<UserEntity, int>
    {
        Task<UserEntity> GetByUsername(string username);
        Task<bool> ExistsByUsername(string username);
    }
}