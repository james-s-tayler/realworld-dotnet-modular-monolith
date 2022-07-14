using FluentMigrator.Runner.Initialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace App.Core.SchemaManagement.Postgres
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

        public string GetConnectionString(string moduleName)
        {
            var database = $"{_configuration[$"DatabaseConfig:{moduleName}:DatabaseName"]}_{_hostEnvironment.EnvironmentName.ToLowerInvariant()}";
            var server = _configuration[$"DatabaseConfig:{moduleName}:Server"];
            var port = _configuration[$"DatabaseConfig:{moduleName}:Port"];
            var userId = _configuration[$"DatabaseConfig:{moduleName}:UserId"];
            var password = _configuration[$"DatabaseConfig:{moduleName}:Password"];

            return $"Server={server};Port={port};Database={database};User Id={userId};Password={password};";
        }

        public int Priority => 0;
    }
}