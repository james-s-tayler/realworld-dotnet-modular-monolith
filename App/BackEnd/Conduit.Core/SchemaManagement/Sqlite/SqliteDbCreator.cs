using System;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Conduit.Core.SchemaManagement.Sqlite
{
    public class SqliteDbCreator : IDbCreator
    {
        private readonly IHostEnvironment _hostEnvironment;
        private readonly IConfiguration _configuration;
        private readonly ILogger<SqliteDbCreator> _logger;

        public SqliteDbCreator(ILogger<SqliteDbCreator> logger, 
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
                var filename = $"{dbName}.db";
                
                if (!File.Exists(filename))
                {
                    SQLiteConnection.CreateFile(filename);
                }
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "Fatal error encountered when trying to create the database.");
                throw;
            }
        }
    }
}