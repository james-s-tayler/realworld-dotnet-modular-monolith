using System;
using System.IO;
namespace Conduit.Core.SchemaManagement.Sqlite
{
    public class SqliteDbCreator : IDbCreator
    {
        public void EnsureCreateDatabase(string moduleName, string dbName)
        {
            var filename = $"{dbName}.db";
                
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