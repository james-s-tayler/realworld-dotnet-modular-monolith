using System.Linq;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Conduit.Core.SchemaManagement.Postgres
{
    public class PostgresDbCreator : IDbCreator
    {
        private readonly IConfiguration _configuration;

        public PostgresDbCreator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void EnsureCreateDatabase(string moduleName, string dbName)
        {
            using var connection = GetMasterDbConnection(moduleName);
            
            if (!DatabaseExists(connection, dbName))
            {
                connection.Execute($"CREATE DATABASE {dbName}");
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