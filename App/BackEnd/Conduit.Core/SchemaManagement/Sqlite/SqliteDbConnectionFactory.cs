using System.Data.Common;
using Dapper.Logging;
using JetBrains.Annotations;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;

namespace Conduit.Core.SchemaManagement.Sqlite
{
    public class SqliteDbConnectionFactory : IDbConnectionFactory
    {
        public string ModuleName { get; }
        private readonly SqliteConnectionStringReader _sqliteConnectionStringReader;

        public SqliteDbConnectionFactory([NotNull]SqliteConnectionStringReader sqliteConnectionStringReader, [NotNull]string moduleName)
        {
            _sqliteConnectionStringReader = sqliteConnectionStringReader;
            ModuleName = moduleName;
        }

        public DbConnection CreateConnection()
        {
            var sqliteConnection = new SqliteConnection(_sqliteConnectionStringReader.GetConnectionString(ModuleName));
            sqliteConnection.Open();
            
            // Enable write-ahead logging - https://docs.microsoft.com/en-us/dotnet/standard/data/sqlite/async
            var walCommand = sqliteConnection.CreateCommand();
            walCommand.CommandText = @"PRAGMA journal_mode=WAL";
            walCommand.ExecuteNonQuery();
            
            return sqliteConnection;
        }
    }
}