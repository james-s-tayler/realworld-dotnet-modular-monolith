using System.Reflection;
using System.Runtime.CompilerServices;
using Application.Core.DataAccess.Dapper.Sqlite;
using Application.Core.Modules;
using Application.Core.SchemaManagement;
using Application.ModuleName.Domain.Contracts;
using Application.ModuleName.Domain.Infrastructure.Repositories;
using Application.ModuleName.Domain.Setup.Module;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

//would be epic to add this in via Fody!
[assembly: HostingStartup(typeof(ModuleNameModule))]
[assembly: InternalsVisibleTo("Application.ModuleName.Domain.Tests.Unit")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace Application.ModuleName.Domain.Setup.Module
{
    internal class ModuleNameModule : AbstractModule
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
            return typeof(ModuleNameModule).Assembly;
        }

        public override Assembly GetModuleContractsAssembly()
        {
            return ModuleNameDomainContracts.Assembly;
        }

        protected override void AddModuleServices(IConfiguration configuration, IServiceCollection services)
        {
            services.AddTransient<IExampleRepository, SqliteExampleRepository>();
        }
    }
}