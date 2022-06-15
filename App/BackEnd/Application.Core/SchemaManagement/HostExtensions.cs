using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Application.Core.SchemaManagement
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase(this IHost host, string dbName)
        {
            using var scope = host.Services.CreateScope();
            throw new NotImplementedException();
            //SchemaManager.RunMigrations(scope, dbName);

            return host;
        }
    }
}