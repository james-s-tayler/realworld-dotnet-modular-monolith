using System.Threading.Tasks;
using App.Content.Domain.Entities;
using App.Core.DataAccess;

namespace App.Content.Domain.Infrastructure.Repositories
{
    internal interface IUserRepository : ICrudRepository<UserEntity, int>
    {
        Task<UserEntity> GetByArticleId(int articleId);
        Task<UserEntity> GetByUsername(string username);
        Task<bool> ExistsByUsername(string username);
    }
}