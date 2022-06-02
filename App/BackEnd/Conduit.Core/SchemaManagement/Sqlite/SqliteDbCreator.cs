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
                File.WriteAllBytes(filename, Array.Empty<byte>());
            }
        }
    }
}