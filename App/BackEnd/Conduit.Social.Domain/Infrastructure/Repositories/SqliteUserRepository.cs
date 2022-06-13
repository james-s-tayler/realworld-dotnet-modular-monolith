using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Conduit.Core.Context;
using Conduit.Core.DataAccess;
using Conduit.Social.Domain.Entities;
using Dapper;
using JetBrains.Annotations;

namespace Conduit.Social.Domain.Infrastructure.Repositories
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

            var arguments = new
            {
                id = id
            };

            var exists = _connection.ExecuteScalar<bool>(sql, arguments);
            
            return Task.FromResult(exists);
        }

        public Task<User> GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<User>> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public Task<int> Create(User user)
        {
            var sql = "INSERT INTO users (id, username) VALUES (@id, @username) RETURNING *";
            var arguments = new { id = user.Id, username = user.Username };

            var insertedUser = _connection.QuerySingle<User>(sql, arguments);
            
            return Task.FromResult(insertedUser.Id);
        }

        public Task Update(User user)
        {
            throw new System.NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> DeleteAll()
        {
            throw new System.NotImplementedException();
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

        public Task<bool> IsFollowing(int userId, int followingUserId)
        {
            var sql = "SELECT EXISTS(SELECT 1 FROM following WHERE user_id=@user_id AND following_user_id=@following_user_id)";
            var arguments = new { user_id = userId, following_user_id = followingUserId };

            var isFollowing = _connection.ExecuteScalar<bool>(sql, arguments);

            return Task.FromResult(isFollowing);
        }
    }
}