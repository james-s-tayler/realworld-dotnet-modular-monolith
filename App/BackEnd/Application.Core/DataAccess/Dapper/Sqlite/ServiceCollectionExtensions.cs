using System;
using Application.Core.Modules;
using Application.Core.SchemaManagement.Sqlite;
using Dapper;
using Dapper.Logging;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Application.Core.DataAccess.Dapper.Sqlite
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSqliteDbConnectionFactory(this IServiceCollection services, IConfiguration configuration, IHostEnvironment hostEnvironment, IModule module)
        {
            services.AddDbConnectionFactory(_ =>
            {
                //this is done because Dapper can't map these types from sqlite without it as sqlite's type system is very basic
                //however, it breaks being able to use different DB providers in different modules if they're both using Dapper
                if (!SqlMapper.HasTypeHandler(typeof(DateTimeOffset)))
                {
                    SqlMapper.AddTypeHandler(new SqliteDateTimeOffsetHandler());
                }
                if (!SqlMapper.HasTypeHandler(typeof(Guid)))
                {
                    SqlMapper.AddTypeHandler(new SqliteGuidHandler());
                }
                if (!SqlMapper.HasTypeHandler(typeof(TimeSpan)))
                {
                    SqlMapper.AddTypeHandler(new SqliteTimeSpanHandler());
                }
                var connectionStringReader = new SqliteConnectionStringReader(configuration, hostEnvironment);
                var sqliteConnection = new SqliteConnection(connectionStringReader!.GetConnectionString(module.GetModuleName()));
                sqliteConnection.Open();
            
                // Enable write-ahead logging - https://docs.microsoft.com/en-us/dotnet/standard/data/sqlite/async
                var walCommand = sqliteConnection.CreateCommand();
                walCommand.CommandText = @"PRAGMA journal_mode=WAL"; //can potentially speed up tests with ;PRAGMA synchronous=OFF 
                walCommand.ExecuteNonQuery();

                return sqliteConnection;
            });
            services.AddScoped(provider =>
            {
                var connectionFactory = provider.GetService<IDbConnectionFactory>();
                return connectionFactory!.CreateConnection();
            });
            
            return services;
        }
        
        public static IServiceCollection AddModuleSqliteDbConnectionFactory<T>(
            this IServiceCollection services, 
            IConfiguration configuration, 
            IHostEnvironment hostEnvironment, 
            T module) where T : class, IModule
        {
            services.AddDbConnectionFactoryWithCtx<T>(_ =>
            {
                //this is done because Dapper can't map these types from sqlite without it as sqlite's type system is very basic
                //however, it breaks being able to use different DB providers in different modules if they're both using Dapper
                if (!SqlMapper.HasTypeHandler(typeof(DateTimeOffset)))
                {
                    SqlMapper.AddTypeHandler(new SqliteDateTimeOffsetHandler());
                }
                if (!SqlMapper.HasTypeHandler(typeof(Guid)))
                {
                    SqlMapper.AddTypeHandler(new SqliteGuidHandler());
                }
                if (!SqlMapper.HasTypeHandler(typeof(TimeSpan)))
                {
                    SqlMapper.AddTypeHandler(new SqliteTimeSpanHandler());
                }
                var connectionStringReader = new SqliteConnectionStringReader(configuration, hostEnvironment);
                var sqliteConnection = new SqliteConnection(connectionStringReader!.GetConnectionString(module.GetModuleName()));
                sqliteConnection.Open();
            
                // Enable write-ahead logging - https://docs.microsoft.com/en-us/dotnet/standard/data/sqlite/async
                var walCommand = sqliteConnection.CreateCommand();
                walCommand.CommandText = @"PRAGMA journal_mode=WAL"; //can potentially speed up tests with ;PRAGMA synchronous=OFF 
                walCommand.ExecuteNonQuery();

                return sqliteConnection;
            });
            services.AddScoped(provider =>
            {
                var connectionFactory = provider.GetService<IDbConnectionFactory<T>>();
                var connection = connectionFactory!.CreateConnection(module);
                return new ModuleDbConnectionWrapper<T>(connection);
            });
            
            return services;
        }
    }
}