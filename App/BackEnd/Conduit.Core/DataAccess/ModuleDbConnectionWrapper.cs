using System.Data;
using System.Data.Common;
using Conduit.Core.Modules;
using JetBrains.Annotations;

namespace Conduit.Core.DataAccess
{
    /// <summary>
    /// We use IDbConnectionFactory<T> to produce a DbConnection for each module T and then register the instance of DbConnection as Scoped by
    /// wrapping it in ModuleDbConnectionWrapper<T> allowing the DI registrations to be unique in order to be able to resolve the correct DbConnection object reliably
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ModuleDbConnectionWrapper<T> where T : class, IModule
    {
        public DbConnection Connection { get; }

        public ModuleDbConnectionWrapper([NotNull] DbConnection wrappedConnection)
        {
            Connection = wrappedConnection;
        }
    }
}