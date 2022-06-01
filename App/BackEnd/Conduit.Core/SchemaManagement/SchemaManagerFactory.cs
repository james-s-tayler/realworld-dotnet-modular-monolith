using System;
using System.Reflection;
using Conduit.Core.SchemaManagement.Postgres;
using Conduit.Core.SchemaManagement.Sqlite;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Initialization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace Conduit.Core.SchemaManagement
{
    public static class SchemaManagerFactory
    {
        public static void RunPostgresMigrations(IConfiguration configuration,
            IWebHostEnvironment hostEnvironment,
            Assembly moduleAssembly)
        {
            RunDatabaseMigrations<PostgresDbCreator, PostgresConnectionStringReader>(
                migrationRunnerBuilder => migrationRunnerBuilder.AddPostgres(),
                hostEnvironment,
                moduleAssembly,
                configuration);
        }
        
        public static void RunSqliteMigrations(IConfiguration configuration,
            IWebHostEnvironment hostEnvironment,
            Assembly moduleAssembly)
        {
            RunDatabaseMigrations<SqliteDbCreator, SqliteConnectionStringReader>(
                migrationRunnerBuilder => migrationRunnerBuilder.AddSQLite(),
                hostEnvironment,
                moduleAssembly,
                configuration);
        }
        
        private static void RunDatabaseMigrations<TDbCreator, TConnectionStringReader>(
            Func<IMigrationRunnerBuilder, IMigrationRunnerBuilder> addDatabaseFunc,
            IWebHostEnvironment hostEnvironment,
            Assembly moduleAssembly, IConfiguration configuration) where TDbCreator : class, IDbCreator where TConnectionStringReader : class, IConnectionStringReader
        {
            var moduleName = moduleAssembly.GetName().Name;
            var databaseName = $"{configuration[$"DatabaseConfig:{moduleName}:DatabaseName"]}_{hostEnvironment.EnvironmentName.ToLowerInvariant()}";
           
            var tempServiceProvider = new ServiceCollection()
                .AddSingleton<IDbCreator, TDbCreator>()
                .AddSingleton<IConnectionStringReader, TConnectionStringReader>()
                .AddLogging(c => c.AddFluentMigratorConsole())
                .AddFluentMigratorCore()
                .ConfigureRunner(migrationRunnerBuilder =>
                {
                    migrationRunnerBuilder
                        .AddDatabase(addDatabaseFunc)
                        .WithGlobalConnectionString(moduleName)
                        .ScanIn(moduleAssembly).For.Migrations();
                })
                .Configure<RunnerOptions>(opt => {
                    opt.Tags = new[] { configuration[$"DatabaseConfig:{moduleName}:DatabaseName"] };
                })
                .BuildServiceProvider(false);
            
            using var scope = tempServiceProvider.CreateScope();
            SchemaManager.RunMigrations(scope, moduleName, databaseName);
        }
    }

    public static class MigrationRunnerBuilderExtensions
    {
        public static IMigrationRunnerBuilder AddDatabase(this IMigrationRunnerBuilder migrationRunnerBuilder, Func<IMigrationRunnerBuilder, IMigrationRunnerBuilder> addDatabaseFunc)
        {
            return addDatabaseFunc.Invoke(migrationRunnerBuilder);
        }
    }
}