namespace Conduit.Core.SchemaManagement
{
    public interface IDbCreator
    {
        void EnsureCreateDatabase(string moduleName, string dbName);
    }
}