using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using App.Content.Domain.Entities;
using App.Content.Domain.Setup.Module;
using App.Core.DataAccess;
using Dapper;
using JetBrains.Annotations;

namespace App.Content.Domain.Infrastructure.Repositories
{
    internal class SqliteUserRepository : IUserRepository
    {
        private readonly DbConnection _connection;

        public SqliteUserRepository([NotNull] ModuleDbConnectionWrapper<ContentModule> connectionWrapper)
        {
            _connection = connectionWrapper.Connection;
        }

        public Task<bool> Exists(int id)
        {
            var sql = "SELECT EXISTS(SELECT 1 FROM users WHERE user_id=@user_id)";

            var arguments = new { user_id = id };

            var exists = _connection.ExecuteScalar<bool>(sql, arguments);

            return Task.FromResult(exists);
        }

        public Task<UserEntity> GetById(int id)
        {
            var sql = "SELECT * FROM users WHERE user_id=@user_id";
            var arguments = new { user_id = id };
            return Task.FromResult(_connection.QuerySingle<UserEntity>(sql, arguments));
        }

        public Task<IEnumerable<UserEntity>> GetAll()
        {
            var sql = "SELECT * FROM users";
            return Task.FromResult(_connection.Query<UserEntity>(sql));
        }

        public Task<int> Create(UserEntity userEntity)
        {
            var sql = "INSERT INTO users (user_id, username) VALUES (@user_id, @username) RETURNING *";
            var arguments = new
            {
                user_id = userEntity.UserId,
                username = userEntity.Username
            };

            var insertedUser = _connection.QuerySingle<UserEntity>(sql, arguments);

            return Task.FromResult(insertedUser.UserId);
        }

        public Task Update(UserEntity userEntity)
        {
            var sql = "UPDATE users SET username = @username WHERE user_id=@user_id";
            var arguments = new
            {
                user_id = userEntity.UserId,
                username = userEntity.Username
            };

            return Task.FromResult(_connection.Execute(sql, arguments));
        }

        public Task Delete(int id)
        {
            var sql = "DELETE FROM users WHERE user_id=@user_id";
            var arguments = new { user_id = id };
            return Task.FromResult(_connection.Execute(sql, arguments));
        }

        public Task<int> DeleteAll()
        {
            var sql = "DELETE FROM users";
            return Task.FromResult(_connection.Execute(sql));
        }

        public Task<UserEntity> GetByArticleId(int articleId)
        {
            var sql = "SELECT u.* FROM users u JOIN articles a ON a.user_id = u.user_id WHERE a.id=@article_id";
            var arguments = new { article_id = articleId };
            return Task.FromResult(_connection.QuerySingle<UserEntity>(sql, arguments));
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
    }
}