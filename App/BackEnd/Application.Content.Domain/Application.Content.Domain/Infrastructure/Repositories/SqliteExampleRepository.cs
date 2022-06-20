using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Application.Core.DataAccess;
using Application.Content.Domain.Entities;
using Application.Content.Domain.Setup.Module;
using Dapper;
using JetBrains.Annotations;

namespace Application.Content.Domain.Infrastructure.Repositories
{
    internal class SqliteExampleRepository : IExampleRepository
    {
        private readonly DbConnection _connection;

        public SqliteExampleRepository([NotNull] ModuleDbConnectionWrapper<ContentModule> connectionWrapper)
        {
            _connection = connectionWrapper.Connection;
        }

        public Task<bool> Exists(int id)
        {
            string sql = "SELECT EXISTS(SELECT 1 FROM example WHERE id=@id)";

            var arguments = new
            {
                id = id
            };

            var exists = _connection.ExecuteScalar<bool>(sql, arguments);

            return Task.FromResult(exists);
        }

        public Task<Example> GetById(int id)
        {
            string sql = "SELECT * FROM example WHERE id=@id";

            var arguments = new
            {
                id = id
            };

            var examples = _connection.Query<Example>(sql, arguments);

            return Task.FromResult(examples.SingleOrDefault());
        }

        public Task<IEnumerable<Example>> GetAll()
        {
            string sql = "SELECT * FROM example";

            return Task.FromResult(_connection.Query<Example>(sql));
        }

        public Task<int> Create([NotNull] Example example)
        {
            var sql = "INSERT INTO example (something) VALUES (@something) RETURNING *";

            var arguments = new
            {
                something = example.Something
            };

            var insertedUser = _connection.QuerySingle<Example>(sql, arguments);

            return Task.FromResult(insertedUser.Id);
        }

        public Task Update([NotNull] Example example)
        {
            var sql = "UPDATE example SET something = @something WHERE id = @id";

            var arguments = new
            {
                id = example.Id,
                username = example.Something
            };

            _connection.Execute(sql, arguments);
            return Task.CompletedTask;
        }

        public Task Delete(int id)
        {
            var sql = "DELETE FROM example WHERE id = @id";

            var arguments = new
            {
                id = id
            };

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