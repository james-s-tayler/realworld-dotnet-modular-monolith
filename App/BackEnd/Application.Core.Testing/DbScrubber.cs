using System;
using System.Data.Common;
using Application.Core.DataAccess;
using Application.Core.SchemaManagement;
using Dapper;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Core.Testing
{
    public static class DbScrubber
    {
        public static void ClearDatabaseTables<T>(this WebApplicationFactory<T> applicationFactory) where  T : class
        {
            using var scope = applicationFactory.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            
            var moduleDbConnections = scope.ServiceProvider.GetServices<IModuleDbConnection>();
            
            //hmm this ain't going to work if you've got foreign keys unless you drop them first
            foreach (var moduleDbConnection in moduleDbConnections)
            {
                switch (moduleDbConnection.Vendor)
                {
                    case DbVendor.Sqlite:
                        ClearSqliteDatabaseTables(moduleDbConnection.Connection);
                        break;
                    default:
                        throw new NotImplementedException($"No DbScrubber implemented for {moduleDbConnection.Vendor}");
                }
            }
        }
        
        private static void ClearSqliteDatabaseTables(DbConnection dbConnection)
        {
            var tables = dbConnection.Query<string>("SELECT name FROM sqlite_schema WHERE type='table' AND name != 'VersionInfo'");

            foreach (var table in tables)
            {
                dbConnection.Execute($"DELETE FROM {table}");
            }
        }
    }
}