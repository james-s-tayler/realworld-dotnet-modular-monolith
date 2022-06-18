using System.Data.Common;
using Application.Core.Modules;
using Application.Core.SchemaManagement;
using JetBrains.Annotations;

namespace Application.Core.DataAccess
{
    /// <summary>
    /// We use IDbConnectionFactory<T> to produce a DbConnection for each module T and then register the instance of DbConnection as Scoped by
    /// wrapping it in ModuleDbConnectionWrapper<T> allowing the DI registrations to be unique in order to be able to resolve the correct DbConnection object reliably
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ModuleDbConnectionWrapper<T> : IModuleDbConnection where T : class, IModule
    {
        public DbConnection Connection { get; }
        public DbVendor Vendor { get; }

        public ModuleDbConnectionWrapper([NotNull] DbConnection wrappedConnection, DbVendor dbVendor)
        {
            Connection = wrappedConnection;
            Vendor = dbVendor;
        }
    }
}