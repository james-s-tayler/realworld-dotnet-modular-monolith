using System.Collections.Generic;
using System.Threading.Tasks;
using App.Social.Domain.Entities;

namespace App.Social.Domain.Infrastructure.Repositories
{
    internal interface IUserRepository
    {
        Task<bool> Exists(int id);
        Task<UserEntity> GetById(int id);
        Task<int> Create(UserEntity userEntity);
        Task Update(UserEntity userEntity);
        Task<UserEntity> GetByUsername(string username);
        Task<bool> ExistsByUsername(string username);
        Task<bool> IsFollowing(int userId, int followUserId);
        Task FollowSelf(int userId);
        Task FollowUser(int userId, int followUserId);
        Task UnfollowUser(int userId, int followUserId);
    }
}