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
            var database = $"{_configuration[$"DatabaseConfig:{moduleName}:DatabaseName"]}_{_hostEnvironment.EnvironmentName}".ToLowerInvariant();
            var filename = $"/sqlite/{database}.db";
            
            return new SqliteConnectionStringBuilder
            {
                DataSource = filename,
                Cache = SqliteCacheMode.Shared,
                ForeignKeys = true,
                Mode = SqliteOpenMode.ReadWriteCreate
            }.ToString();
        }

        public int Priority => 0;
    }
}