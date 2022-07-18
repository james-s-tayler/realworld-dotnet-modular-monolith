using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using App.Core.DataAccess;
using App.ModuleName.Domain.Entities;
using App.ModuleName.Domain.Setup.Module;
using Dapper;
using JetBrains.Annotations;

namespace App.ModuleName.Domain.Infrastructure.Repositories
{
    internal class SqliteExampleRepository : IExampleRepository
    {
        private readonly DbConnection _connection;

        public SqliteExampleRepository([NotNull] ModuleDbConnectionWrapper<ModuleNameModule> connectionWrapper)
        {
            _connection = connectionWrapper.Connection;
        }
        
        public Task<bool> Exists(int id)
        {
            string sql = "SELECT EXISTS(SELECT 1 FROM example WHERE id=@id)";
    
            var arguments = new { id };
            
            var exists = _connection.ExecuteScalar<bool>(sql, arguments);
            
            return Task.FromResult(exists);
        }

        public Task<ExampleEntity> GetById(int id)
        {
            string sql = "SELECT * FROM example WHERE id=@id";
    
            var arguments = new { id };
            
            var example = _connection.QuerySingleOrDefault<ExampleEntity>(sql, arguments);
            
            return Task.FromResult(example);
        }

        public Task<IEnumerable<ExampleEntity>> GetAll()
        {
            string sql = "SELECT * FROM example";

            return Task.FromResult(_connection.Query<ExampleEntity>(sql));
        }

        public Task<int> Create([NotNull] ExampleEntity exampleEntity)
        {
            var sql = "INSERT INTO example (something) VALUES (@something) RETURNING *";

            var arguments = new
            {
                something = exampleEntity.Something
            };
            
            var insertedUser = _connection.QuerySingle<ExampleEntity>(sql, arguments);
            
            return Task.FromResult(insertedUser.Id);
        }

        public Task Update([NotNull] ExampleEntity exampleEntity)
        {
            var sql = "UPDATE example SET something = @something WHERE id = @id";

            var arguments = new
            {
                id = exampleEntity.Id,
                something = exampleEntity.Something
            };
            
            _connection.Execute(sql, arguments);
            return Task.CompletedTask;
        }

        public Task Delete(int id)
        {
            var sql = "DELETE FROM example WHERE id = @id";

            var arguments = new { id };

            _connection.Execute(sql, arguments);
            return Task.CompletedTask;
        }

        public Task<int> DeleteAll()
        {
            var sql = "DELETE FROM example";

            return Task.FromResult(_connection.Execute(sql));
        }
    }
}