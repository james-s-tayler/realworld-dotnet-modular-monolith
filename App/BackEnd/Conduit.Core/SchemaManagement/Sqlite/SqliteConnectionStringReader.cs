using FluentMigrator.Runner.Initialization;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Conduit.Core.SchemaManagement.Sqlite
{
    public class SqliteConnectionStringReader : IConnectionStringReader
    {
        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment _hostEnvironment;

        public SqliteConnectionStringReader(IConfiguration configuration, 
            IHostEnvironment hostEnvironment)
        {
            _configuration = configuration;
            _hostEnvironment = hostEnvironment;
        }

        public string GetConnectionString(string moduleName)
        {
            var database = $"{_configuration[$"DatabaseConfig:{moduleName}:DatabaseName"]}_{_hostEnvironment.EnvironmentName.ToLowerInvariant()}";
            var filename = $"{database}.db";
            
            var connectionString = new SqliteConnectionStringBuilder
            {
                DataSource = filename,
                Cache = SqliteCacheMode.Shared,
                ForeignKeys = true,
                Mode = SqliteOpenMode.ReadWriteCreate,
                Password = _configuration[$"DatabaseConfig:{moduleName}:Password"]
            }.ToString();

            return connectionString;
        }

        public int Priority => 0;
    }
}