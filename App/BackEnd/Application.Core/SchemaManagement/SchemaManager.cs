using System;
using System.Reflection;
using Application.Core.SchemaManagement.Postgres;
using Application.Core.SchemaManagement.Sqlite;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Initialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Application.Core.SchemaManagement
{
    //add two different dbs https://stackoverflow.com/questions/58110840/fluentmigrator-two-sql-databases
    public class SchemaManager
    {
        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly Assembly _moduleAssembly;

        public SchemaManager(IConfiguration configuration, 
            IHostEnvironment hostEnvironment, 
            Assembly moduleAssembly)
        {
            _configuration = configuration;
            _hostEnvironment = hostEnvironment;
            _moduleAssembly = moduleAssembly;
        }

        public void RunPostgresMigrations()
        {
            RunDatabaseMigrations<PostgresDbCreator, PostgresConnectionStringReader>(
                migrationRunnerBuilder => migrationRunnerBuilder.AddPostgres(),
                DbConstants.Postgres);
        }
        
        public void RunSqliteMigrations()
        {
            RunDatabaseMigrations<SqliteDbCreator, SqliteConnectionStringReader>(
                migrationRunnerBuilder => migrationRunnerBuilder.AddSQLite(),
                DbConstants.SQLite);
        }
        
        private void RunDatabaseMigrations<TDbCreator, TConnectionStringReader>(
            Func<IMigrationRunnerBuilder, IMigrationRunnerBuilder> addDatabaseFunc, string vendorTag) 
            where TDbCreator : class, IDbCreator where TConnectionStringReader : class, IConnectionStringReader
        {
            var moduleName = _moduleAssembly.GetName().Name;

            var tempServiceProvider = new ServiceCollection()
                .AddSingleton(_configuration)
                .AddSingleton(_hostEnvironment)
                .AddSingleton<IDbCreator, TDbCreator>()
                .AddSingleton<IConnectionStringReader, TConnectionStringReader>()
                .AddLogging(c => c.AddFluentMigratorConsole())
                .Configure<FluentMigratorLoggerOptions>(o =>
                {
                    o.ShowSql = _configuration.GetValue<bool>("DatabaseConfig:FluentMigratorOptions:ShowSql");
                    o.ShowElapsedTime = _configuration.GetValue<bool>("DatabaseConfig:FluentMigratorOptions:ShowElapsedTime");
                })
                .AddFluentMigratorCore()
                .ConfigureRunner(migrationRunnerBuilder =>
                {
                    migrationRunnerBuilder
                        .AddDatabase(addDatabaseFunc)
                        .WithGlobalConnectionString(moduleName)
                        .ScanIn(_moduleAssembly).For.Migrations();
                })
                .Configure<RunnerOptions>(opt => {
                    opt.Tags = new[] { vendorTag };
                })
                .BuildServiceProvider(false);
            
            using var scope = tempServiceProvider.CreateScope();
            
            var dbCreator = scope.ServiceProvider.GetRequiredService<IDbCreator>();
            var migrationRunner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
            
            dbCreator.EnsureCreateDatabase(_configuration, _hostEnvironment, moduleName);
            migrationRunner.ListMigrations();
            
            if (migrationRunner.HasMigrationsToApplyUp())
            {
                migrationRunner.MigrateUp();
            }
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