using FluentMigrator.Runner.Initialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Conduit.Core.SchemaManagement.Postgres
{
    public class PostgresConnectionStringReader : IConnectionStringReader
    {
        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment _hostEnvironment;

        public PostgresConnectionStringReader(IConfiguration configuration, 
            IHostEnvironment hostEnvironment)
        {
            _configuration = configuration;
            _hostEnvironment = hostEnvironment;
        }

        public string GetConnectionString(string connectionStringOrName)
        {
            var isMasterDb = connectionStringOrName.Equals("MasterSqlConnection");
            
            var database = isMasterDb ? "postgres" : $"{_configuration["DatabaseConfig:DatabaseName"]}_{_hostEnvironment.EnvironmentName.ToLowerInvariant()}";
            var server = _configuration["DatabaseConfig:Server"];
            var port = _configuration["DatabaseConfig:Port"];
            var userId = _configuration["DatabaseConfig:UserId"];
            var password = _configuration["DatabaseConfig:Password"];

            return $"Server={server};Port={port};Database={database};User Id={userId};Password={password};";
        }

        public int Priority => 0;
    }
}