using System.Reflection;
using System.Runtime.CompilerServices;
using App.Core.DataAccess.Dapper.Sqlite;
using App.Core.Modules;
using App.Core.SchemaManagement;
using App.Social.Domain.Contracts;
using App.Social.Domain.Infrastructure.Repositories;
using App.Social.Domain.Setup.Module;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

//would be epic to add this in via Fody!
[assembly: HostingStartup(typeof(SocialModule))]
[assembly: InternalsVisibleTo("App.Social.Domain.Tests.Unit")]
namespace App.Social.Domain.Setup.Module
{
    internal class SocialModule : AbstractModule
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
            return typeof(SocialModule).Assembly;
        }

        public override Assembly GetModuleContractsAssembly()
        {
            return SocialDomainContracts.Assembly;
        }

        protected override void AddModuleServices(IConfiguration configuration, IServiceCollection services)
        {
            services.AddTransient<IUserRepository, SqliteUserRepository>();
            services.AddHttpContextAccessor();
        }
    }
}
