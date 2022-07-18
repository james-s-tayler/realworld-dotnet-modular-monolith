using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using App.Core.DataAccess;
using App.Social.Domain.Entities;
using App.Social.Domain.Setup.Module;
using Dapper;
using JetBrains.Annotations;

namespace App.Social.Domain.Infrastructure.Repositories
{
    internal class SqliteUserRepository : IUserRepository
    {
        private readonly DbConnection _connection;

        public SqliteUserRepository([NotNull] ModuleDbConnectionWrapper<SocialModule> connectionWrapper)
        {
            _connection = connectionWrapper.Connection;
        }
        
        public Task<bool> Exists(int id)
        {
            var sql = "SELECT EXISTS(SELECT 1 FROM users WHERE id=@id)";

            var arguments = new { id };

            var exists = _connection.ExecuteScalar<bool>(sql, arguments);
            
            return Task.FromResult(exists);
        }

        public Task<UserEntity> GetById(int id)
        {
            var sql = "SELECT * FROM users WHERE id=@id";
            var arguments = new {id};
            return Task.FromResult(_connection.QuerySingle<UserEntity>(sql, arguments));
        }

        public Task<int> Create(UserEntity userEntity)
        {
            var sql = "INSERT INTO users (id, username, image, bio) VALUES (@id, @username, @image, @bio) RETURNING *";
            var arguments = new
            {
                id = userEntity.Id, 
                username = userEntity.Username,
                image = userEntity.Image,
                bio = userEntity.Bio
            };

            var insertedUser = _connection.QuerySingle<UserEntity>(sql, arguments);
            
            return Task.FromResult(insertedUser.Id);
        }

        public Task Update(UserEntity userEntity)
        {
            var sql = "UPDATE users SET username = @username, image = @image, bio = @bio WHERE id=@id";
            var arguments = new
            {
                id = userEntity.Id,
                username = userEntity.Username,
                image = userEntity.Image,
                bio = userEntity.Bio
            };

            return Task.FromResult(_connection.Execute(sql, arguments));
        }

        public Task<UserEntity> GetByUsername(string username)
        {
            var sql = "SELECT * FROM users WHERE username=@username";
            var arguments = new { username };

            var user = _connection.QuerySingle<UserEntity>(sql, arguments);

            return Task.FromResult(user);
        }

        public Task<bool> ExistsByUsername(string username)
        {
            var sql = "SELECT EXISTS(SELECT 1 FROM users WHERE username=@username)";
            var arguments = new { username };

            return Task.FromResult(_connection.ExecuteScalar<bool>(sql, arguments));
        }

        public Task<bool> IsFollowing(int userId, int followUserId)
        {
            var sql = "SELECT EXISTS(SELECT 1 FROM followers WHERE user_id=@user_id AND follow_user_id=@follow_user_id)";
            var arguments = new { user_id = userId, follow_user_id = followUserId };

            var isFollowing = _connection.ExecuteScalar<bool>(sql, arguments);

            return Task.FromResult(isFollowing);
        }

        public async Task FollowSelf(int userId)
        {
            await FollowUser(userId, userId);
        }
        
        public Task FollowUser(int userId, int followUserId)
        {
            var sql = "INSERT OR IGNORE INTO followers(user_id, follow_user_id) VALUES(@user_id, @follow_user_id)";
            var arguments = new
            {
                user_id = userId,
                follow_user_id = followUserId
            };
            
            return Task.FromResult(_connection.Execute(sql, arguments));
        }

        public Task UnfollowUser(int userId, int followUserId)
        {
            var sql = "DELETE FROM followers WHERE user_id=@user_id AND follow_user_id=@follow_user_id";
            var arguments = new
            {
                user_id = userId,
                follow_user_id = followUserId
            };

            return Task.FromResult(_connection.Execute(sql, arguments));
        }
    }
}