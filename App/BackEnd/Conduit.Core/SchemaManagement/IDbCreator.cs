namespace Conduit.Core.SchemaManagement
{
    public interface IDbCreator
    {
        void EnsureCreateDatabase(string name);
    }
}