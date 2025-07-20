using System.Collections.Generic;
using System.Threading.Tasks;
using App.Users.Domain.Entities;

namespace App.Users.Domain.Infrastructure.Repositories
{
    internal interface IUserRepository
    {
        Task<bool> Exists(int id);
        Task<UserEntity> GetById(int id);
        Task<IEnumerable<UserEntity>> GetAll();
        Task<int> Create(UserEntity userEntity);
        Task Update(UserEntity userEntity);
        Task Delete(int id);
        Task<int> DeleteAll();
        Task<UserEntity> GetByEmail(string email);
        Task<UserEntity> GetByUsername(string username);
        Task<bool> ExistsByUsername(string username);
        Task<bool> ExistsByEmail(string email);
        Task<bool> IsFollowing(int userId, int followUserId);
        Task FollowUser(int userId, int followUserId);
        Task UnfollowUser(int userId, int followUserId);
    }
}