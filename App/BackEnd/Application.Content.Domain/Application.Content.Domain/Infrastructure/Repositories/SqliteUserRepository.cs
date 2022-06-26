using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Application.Content.Domain.Entities;
using Application.Content.Domain.Setup.Module;
using Application.Core.DataAccess;
using Dapper;
using JetBrains.Annotations;

namespace Application.Content.Domain.Infrastructure.Repositories
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
            var arguments = new {user_id = id};
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
                user_id = userEntity.Id, 
                username = userEntity.Username
            };

            var insertedUser = _connection.QuerySingle<UserEntity>(sql, arguments);
            
            return Task.FromResult(insertedUser.Id);
        }

        public Task Update(UserEntity userEntity)
        {
            var sql = "UPDATE users SET username = @username WHERE user_id=@user_id";
            var arguments = new
            {
                user_id = userEntity.Id,
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