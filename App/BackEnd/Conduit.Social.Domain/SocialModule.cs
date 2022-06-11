using System.Reflection;
using System.Runtime.CompilerServices;
using Conduit.Core.DataAccess.Dapper.Sqlite;
using Conduit.Core.Modules;
using Conduit.Core.SchemaManagement;
using Conduit.Social.Domain;
using Conduit.Social.Domain.Contracts;
using Conduit.Social.Domain.Infrastructure.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

//would be epic to add this in via Fody!
[assembly: HostingStartup(typeof(SocialModule))]
[assembly: InternalsVisibleTo("Conduit.Social.Domain.Tests.Unit")]
namespace Conduit.Social.Domain
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
            return SocialDomain.Assembly;
        }

        public override Assembly GetModuleContractsAssembly()
        {
            return SocialDomainContracts.Assembly;
        }

        protected override void AddModuleServices(IConfiguration configuration, IServiceCollection services)
        {
            services.AddTransient<IUserRepository, SqliteUserRepository>();
        }
    }
}