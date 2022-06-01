using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace Conduit.Core.SchemaManagement
{
    public static class SchemaManager
    {
        public static void RunMigrations(IServiceScope scope, string moduleName, string dbName)
        {
            var dbCreator = scope.ServiceProvider.GetRequiredService<IDbCreator>();
            var migrationRunner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
            
            dbCreator.EnsureCreateDatabase(moduleName, dbName);
            migrationRunner.ListMigrations();
            
            if (migrationRunner.HasMigrationsToApplyUp())
            {
                migrationRunner.MigrateUp();
            }
        }
    }
}