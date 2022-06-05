using System.Linq;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Npgsql;

namespace Conduit.Core.SchemaManagement.Postgres
{
    public class PostgresDbCreator : IDbCreator
    {
        
        public void EnsureCreateDatabase(IConfiguration configuration, IHostEnvironment hostEnvironment, string moduleName)
        {
            using var connection = GetMasterDbConnection(configuration, moduleName);
            var dbName = $"{configuration[$"DatabaseConfig:{moduleName}:DatabaseName"]}_{hostEnvironment.EnvironmentName}".ToLowerInvariant();
            
            if (!DatabaseExists(connection, dbName))
            {
                connection.Execute($"CREATE DATABASE {dbName}");
            }
        }
        
        private NpgsqlConnection GetMasterDbConnection(IConfiguration configuration, string moduleName)
        {
            var database = "postgres";
            var server = configuration[$"DatabaseConfig:{moduleName}:Server"];
            var port = configuration[$"DatabaseConfig:{moduleName}:Port"];
            var userId = configuration[$"DatabaseConfig:{moduleName}:UserId"];
            var password = configuration[$"DatabaseConfig:{moduleName}:Password"];

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