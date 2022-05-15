using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace Conduit.Core.SchemaManagement
{
    public static class SchemaManager
    {
        public static void RunMigrations(IServiceScope scope, string dbName)
        {
            var schemaManager = scope.ServiceProvider.GetRequiredService<DbCreator>();
            var migrationRunner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

            schemaManager.EnsureCreateDatabase(dbName);
            migrationRunner.ListMigrations();
            if (migrationRunner.HasMigrationsToApplyUp())
            {
                migrationRunner.MigrateUp();
            }
        }
    }
}