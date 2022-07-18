using System.Collections.Generic;
using System.Threading.Tasks;
using App.Content.Domain.Entities;

namespace App.Content.Domain.Infrastructure.Repositories
{
    internal interface IUserRepository
    {
        Task<UserEntity> GetByArticleId(int articleId);
        Task<UserEntity> GetByUsername(string username);
        Task<bool> ExistsByUsername(string username);

        Task<bool> Exists(int id);

        Task<UserEntity> GetById(int id);

        Task<IEnumerable<UserEntity>> GetAll();

        Task<int> Create(UserEntity userEntity);

        Task Update(UserEntity userEntity);

        Task Delete(int id);

        Task<int> DeleteAll();
    }
}