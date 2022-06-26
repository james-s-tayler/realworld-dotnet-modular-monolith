using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Application.Core.DataAccess;
using Application.Users.Domain.Entities;
using Application.Users.Domain.Setup.Module;
using Dapper;
using JetBrains.Annotations;

namespace Application.Users.Domain.Infrastructure.Repositories
{
    internal class SqliteUserRepository : IUserRepository
    {
        private readonly DbConnection _connection;

        public SqliteUserRepository([NotNull] ModuleDbConnectionWrapper<UsersModule> connectionWrapper)
        {
            _connection = connectionWrapper.Connection;
        }

        //use sqlite - maybe even try litestream? https://news.ycombinator.com/item?id=31318708
        public Task<bool> Exists(int id)
        {
            string sql = "SELECT EXISTS(SELECT 1 FROM users WHERE id=@id)";
    
            var arguments = new
            {
                id = id
            };
            
            var exists = _connection.ExecuteScalar<bool>(sql, arguments);
            
            return Task.FromResult(exists);
        }

        public Task<UserEntity> GetById(int id)
        {
            string sql = "SELECT * FROM users WHERE id=@id";
    
            var arguments = new
            {
                id = id
            };
            
            var user = _connection.Query<UserEntity>(sql, arguments);
            
            return Task.FromResult(user.SingleOrDefault());
        }

        public Task<IEnumerable<UserEntity>> GetAll()
        {
            string sql = "SELECT * FROM users";

            return Task.FromResult(_connection.Query<UserEntity>(sql));
        }

        public Task<int> Create([NotNull] UserEntity userEntity)
        {
            var sql = "INSERT INTO users (username, email, password, image, bio) VALUES (@username, @email, @password, @image, @bio) RETURNING *";

            var arguments = new
            {
                username = userEntity.Username,
                email = userEntity.Email,
                password = userEntity.Password,
                image = userEntity.Image,
                bio = userEntity.Bio
            };
            
            var insertedUser = _connection.QuerySingle<UserEntity>(sql, arguments);
            
            return Task.FromResult(insertedUser.Id);
        }

        public Task Update([NotNull] UserEntity userEntity)
        {
            var sql = "UPDATE users SET username = @username, email = @email, password = @password, image = @image, bio = @bio WHERE id = @id";

            var arguments = new
            {
                id = userEntity.Id,
                username = userEntity.Username,
                email = userEntity.Email,
                password = userEntity.Password,
                image = userEntity.Image,
                bio = userEntity.Bio
            };
            
            _connection.Execute(sql, arguments);
            return Task.CompletedTask;
        }

        public Task Delete(int id)
        {
            var sql = "DELETE FROM users WHERE id = @id";

            var arguments = new
            {
                id = id
            };

            _connection.Execute(sql, arguments);
            return Task.CompletedTask;
        }

        public Task<int> DeleteAll()
        {
            var sql = "DELETE FROM users";

            return Task.FromResult(_connection.Execute(sql));
        }

        public Task<UserEntity> GetByEmail([NotNull] string email)
        {
            var sql = "SELECT * FROM users WHERE email = @email";

            var arguments = new { email };
            
            return Task.FromResult(_connection.Query<UserEntity>(sql, arguments).SingleOrDefault());
        }

        public Task<UserEntity> GetByUsername(string username)
        {
            var sql = "SELECT * FROM users WHERE username=@username";
            var arguments = new { username };

            return Task.FromResult(_connection.Query<UserEntity>(sql, arguments).SingleOrDefault());
        }

        public Task<bool> ExistsByUsername(string username)
        {
            var sql = "SELECT EXISTS(SELECT 1 FROM users WHERE username=@username)";

            var arguments = new { username };
            
            return Task.FromResult(_connection.ExecuteScalar<bool>(sql, arguments));
        }

        public Task<bool> ExistsByEmail([NotNull] string email)
        {
            var sql = "SELECT EXISTS(SELECT 1 FROM users WHERE email=@email)";

            var arguments = new { email };
            
            return Task.FromResult(_connection.ExecuteScalar<bool>(sql, arguments));
        }
    }
}