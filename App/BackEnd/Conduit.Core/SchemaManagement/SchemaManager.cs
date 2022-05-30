using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace Conduit.Core.SchemaManagement
{
    public static class SchemaManager
    {
        public static void RunMigrations(IServiceScope scope, string dbName)
        {
            var dbCreator = scope.ServiceProvider.GetRequiredService<IDbCreator>();
            dbCreator.EnsureCreateDatabase(dbName);
            
            var migrationRunner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
            
            migrationRunner.ListMigrations();
            if (migrationRunner.HasMigrationsToApplyUp())
            {
                migrationRunner.MigrateUp();
            }
        }
    }
}