using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
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
            var sql = "INSERT INTO users (id) VALUES (@id) RETURNING *";
            var arguments = new { id = user.Id };

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
    }
}