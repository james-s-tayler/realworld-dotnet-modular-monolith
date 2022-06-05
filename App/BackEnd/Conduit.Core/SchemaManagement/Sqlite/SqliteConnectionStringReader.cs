using FluentMigrator.Runner.Initialization;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Conduit.Core.SchemaManagement.Sqlite
{
    //interesting bedtime reading: https://stackoverflow.com/questions/1711631/improve-insert-per-second-performance-of-sqlite?rq=1
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
            var runningInDocker = _configuration.GetValue<bool>("DOTNET_RUNNING_IN_CONTAINER");
            var dbName = $"{_configuration[$"DatabaseConfig:{moduleName}:DatabaseName"]}_{_hostEnvironment.EnvironmentName}".ToLowerInvariant();
            var filename = runningInDocker ? $"/sqlite/{dbName}.db" : $"{dbName}.db";
            
            return new SqliteConnectionStringBuilder
            {
                DataSource = filename,
                Cache = SqliteCacheMode.Shared,
                ForeignKeys = true,
                Mode = SqliteOpenMode.ReadWriteCreate,
                Pooling = true
            }.ToString();
        }

        public int Priority => 0;
    }
}