using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Application.Core.SchemaManagement.Sqlite
{
    public class SqliteDbCreator : IDbCreator
    {
        public void EnsureCreateDatabase(IConfiguration configuration, IHostEnvironment hostEnvironment, string moduleName)
        {
            var runningInDocker = configuration.GetValue<bool>("DOTNET_RUNNING_IN_CONTAINER");
            var dbName = $"{configuration[$"DatabaseConfig:{moduleName}:DatabaseName"]}_{hostEnvironment.EnvironmentName}".ToLowerInvariant();
            var filename = runningInDocker ? $"/sqlite/{dbName}.db" : $"{dbName}.db";

            if (!File.Exists(filename))
            {
                Console.WriteLine($"Database {filename} does not exist - creating it...");
                File.WriteAllBytes(filename, Array.Empty<byte>());
            }
            
            if (!File.Exists(filename))
            {
                throw new Exception($"Database {filename} has not been created");
            }
        }
    }
}