using System.Reflection;
using System.Runtime.CompilerServices;
using Application.Core.DataAccess.Dapper.Sqlite;
using Application.Core.Modules;
using Application.Core.SchemaManagement;
using Application.Feed.Domain.Contracts;
using Application.Feed.Domain.Infrastructure.Repositories;
using Application.Feed.Domain.Setup.Module;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

//would be epic to add this in via Fody!
[assembly: HostingStartup(typeof(FeedModule))]
[assembly: InternalsVisibleTo("Application.Feed.Domain.Tests.Unit")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace Application.Feed.Domain.Setup.Module
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