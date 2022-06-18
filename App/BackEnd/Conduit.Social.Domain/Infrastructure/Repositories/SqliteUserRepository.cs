using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Application.Core.Context;
using Application.Core.DataAccess;
using Conduit.Social.Domain.Entities;
using Dapper;
using JetBrains.Annotations;

namespace Conduit.Social.Domain.Infrastructure.Repositories
{
    internal class SqliteUserRepository : IUserRepository
    {
        private readonly DbConnection _connection;
        private readonly IUserContext _userContext;

        public SqliteUserRepository([NotNull] ModuleDbConnectionWrapper<SocialModule> connectionWrapper, 
            [NotNull] IUserContext userContext)
        {
            _userContext = userContext;
            _connection = connectionWrapper.Connection;
        }
        
        public Task<bool> Exists(int id)
        {
            var sql = "SELECT EXISTS(SELECT 1 FROM users WHERE id=@id)";

            var arguments = new { id };

            var exists = _connection.ExecuteScalar<bool>(sql, arguments);
            
            return Task.FromResult(exists);
        }

        public Task<User> GetById(int id)
        {
            var sql = "SELECT * FROM users WHERE id=@id";
            var arguments = new {id};
            return Task.FromResult(_connection.QuerySingle<User>(sql, arguments));
        }

        public Task<IEnumerable<User>> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public Task<int> Create(User user)
        {
            var sql = "INSERT INTO users (id, username, image, bio) VALUES (@id, @username, @image, @bio) RETURNING *";
            var arguments = new
            {
                id = user.Id, 
                username = user.Username,
                image = user.Image,
                bio = user.Bio
            };

            var insertedUser = _connection.QuerySingle<User>(sql, arguments);
            
            return Task.FromResult(insertedUser.Id);
        }

        public Task Update(User user)
        {
            var sql = "UPDATE users SET username = @username, image = @image, bio = @bio WHERE id=@id";
            var arguments = new
            {
                id = user.Id,
                username = user.Username,
                image = user.Image,
                bio = user.Bio
            };

            return Task.FromResult(_connection.Execute(sql, arguments));
        }

        public Task Delete(int id)
        {
            var sql = "DELETE FROM users WHERE id=@id";
            var arguments = new { id };
            return Task.FromResult(_connection.Execute(sql, arguments));
        }

        public Task<int> DeleteAll()
        {
            var sql = "DELETE FROM users";
            _connection.Execute(sql);

            sql = "DELETE FROM followers";
            return Task.FromResult(_connection.Execute(sql));
        }

        public Task<User> GetByUsername(string username)
        {
            var sql = "SELECT * FROM users WHERE username=@username";
            var arguments = new { username };

            var user = _connection.QuerySingle<User>(sql, arguments);

            return Task.FromResult(user);
        }

        public Task<bool> ExistsByUsername(string username)
        {
            var sql = "SELECT EXISTS(SELECT 1 FROM users WHERE username=@username)";
            var arguments = new { username };

            return Task.FromResult(_connection.ExecuteScalar<bool>(sql, arguments));
        }

        public Task<bool> IsFollowing(int followUserId)
        {
            var sql = "SELECT EXISTS(SELECT 1 FROM followers WHERE user_id=@user_id AND follow_user_id=@follow_user_id)";
            var arguments = new { user_id = _userContext.UserId, follow_user_id = followUserId };

            var isFollowing = _connection.ExecuteScalar<bool>(sql, arguments);

            return Task.FromResult(isFollowing);
        }

        public Task FollowUser(int followUserId)
        {
            var sql = "INSERT OR IGNORE INTO followers(user_id, follow_user_id) VALUES(@user_id, @follow_user_id)";
            var arguments = new
            {
                user_id = _userContext.UserId,
                follow_user_id = followUserId
            };
            
            return Task.FromResult(_connection.Execute(sql, arguments));
        }

        public Task UnfollowUser(int followUserId)
        {
            var sql = "DELETE FROM followers WHERE user_id=@user_id AND follow_user_id=@follow_user_id";
            var arguments = new
            {
                user_id = _userContext.UserId,
                follow_user_id = followUserId
            };

            return Task.FromResult(_connection.Execute(sql, arguments));
        }
    }
}