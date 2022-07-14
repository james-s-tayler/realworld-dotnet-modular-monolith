using System.Reflection;
using System.Runtime.CompilerServices;
using App.Core.DataAccess.Dapper.Sqlite;
using App.Core.Modules;
using App.Core.SchemaManagement;
using App.Feed.Domain.Contracts;
using App.Feed.Domain.Infrastructure.Repositories;
using App.Feed.Domain.Setup.Module;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

//would be epic to add this in via Fody!
[assembly: HostingStartup(typeof(FeedModule))]
[assembly: InternalsVisibleTo("App.Feed.Domain.Tests.Unit")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace App.Feed.Domain.Setup.Module
{
    internal class FeedModule : AbstractModule
    {
        protected override void AddDbConnectionFactory(IServiceCollection services, IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            services.AddModuleSqliteDbConnectionFactory(configuration, hostEnvironment, this);
        }

        protected override void RunModuleDatabaseMigrations(SchemaManager schemaManager)
        {
            schemaManager.RunSqliteMigrations();
        }
        
        public override Assembly GetModuleAssembly()
        {
            return typeof(FeedModule).Assembly;
        }

        public override Assembly GetModuleContractsAssembly()
        {
            return FeedDomainContracts.Assembly;
        }

        protected override void AddModuleServices(IConfiguration configuration, IServiceCollection services)
        {
            services.AddTransient<IExampleRepository, SqliteExampleRepository>();
        }
    }
}