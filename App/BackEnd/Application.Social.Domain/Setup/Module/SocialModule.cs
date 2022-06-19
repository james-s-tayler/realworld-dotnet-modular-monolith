using System.Reflection;
using System.Runtime.CompilerServices;
using Application.Core.DataAccess.Dapper.Sqlite;
using Application.Core.Modules;
using Application.Core.SchemaManagement;
using Application.Social.Domain.Contracts;
using Application.Social.Domain.Infrastructure.Repositories;
using Application.Social.Domain.Setup.Module;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

//would be epic to add this in via Fody!
[assembly: HostingStartup(typeof(SocialModule))]
[assembly: InternalsVisibleTo("Application.Social.Domain.Tests.Unit")]
namespace Application.Social.Domain.Setup.Module
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
