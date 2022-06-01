using System;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Conduit.Core.SchemaManagement.Postgres
{
    public class PostgresDbCreator : IDbCreator
    {
        private readonly IHostEnvironment _hostEnvironment;
        private readonly IConfiguration _configuration;
        private readonly ILogger<PostgresDbCreator> _logger;

        public PostgresDbCreator(ILogger<PostgresDbCreator> logger, 
            IHostEnvironment hostEnvironment, 
            IConfiguration configuration)
        {
            _logger = logger;
            _hostEnvironment = hostEnvironment;
            _configuration = configuration;
        }

        public void EnsureCreateDatabase(string moduleName, string dbName)
        {
            try
            {
                using var connection = GetMasterDbConnection(moduleName);
            
                if (!DatabaseExists(connection, dbName))
                {
                    connection.Execute($"CREATE DATABASE {dbName}");
                }
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "Fatal error encountered when trying to create the database.");
                throw;
            }
        }
        
        private NpgsqlConnection GetMasterDbConnection(string moduleName)
        {
            var database = "postgres";
            var server = _configuration[$"DatabaseConfig:{moduleName}:Server"];
            var port = _configuration[$"DatabaseConfig:{moduleName}:Port"];
            var userId = _configuration[$"DatabaseConfig:{moduleName}:UserId"];
            var password = _configuration[$"DatabaseConfig:{moduleName}:Password"];

            var connectionString = $"Server={server};Port={port};Database={database};User Id={userId};Password={password};";
            return new NpgsqlConnection(connectionString);
        }

        private bool DatabaseExists(NpgsqlConnection connection, string dbName)
        {
            var query = "SELECT * FROM pg_database WHERE datname = @name";
            var parameters = new DynamicParameters();
            parameters.Add("name", dbName);
                
            var records = connection.Query(query, parameters);
                
            return records.Any();
        }
    }
}