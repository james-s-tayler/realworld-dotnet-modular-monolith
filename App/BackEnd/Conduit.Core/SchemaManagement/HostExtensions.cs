using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Conduit.Core.SchemaManagement
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase(this IHost host, string dbName)
        {
            using var scope = host.Services.CreateScope();
            SchemaManager.RunMigrations(scope, dbName);

            return host;
        }
    }
}