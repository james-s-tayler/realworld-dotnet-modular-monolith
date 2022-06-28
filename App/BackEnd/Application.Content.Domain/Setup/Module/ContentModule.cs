using System.Reflection;
using System.Runtime.CompilerServices;
using Application.Core.DataAccess.Dapper.Sqlite;
using Application.Core.Modules;
using Application.Core.SchemaManagement;
using Application.Content.Domain.Contracts;
using Application.Content.Domain.Infrastructure.Repositories;
using Application.Content.Domain.Infrastructure.Services;
using Application.Content.Domain.Setup.Module;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

//would be epic to add this in via Fody!
[assembly: HostingStartup(typeof(ContentModule))]
[assembly: InternalsVisibleTo("Application.Content.Domain.Tests.Unit")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace Application.Content.Domain.Setup.Module
{
    internal class ContentModule : AbstractModule
    {
        protected override void AddDbConnectionFactory(IServiceCollection services, IConfiguration configuration,
            IHostEnvironment hostEnvironment)
        {
            services.AddModuleSqliteDbConnectionFactory(configuration, hostEnvironment, this);
        }

        protected override void RunModuleDatabaseMigrations(SchemaManager schemaManager)
        {
            schemaManager.RunSqliteMigrations();
        }

        public override Assembly GetModuleAssembly()
        {
            return typeof(ContentModule).Assembly;
        }

        public override Assembly GetModuleContractsAssembly()
        {
            return ContentDomainContracts.Assembly;
        }

        protected override void AddModuleServices(IConfiguration configuration, IServiceCollection services)
        {
            services.AddTransient<IArticleRepository, SqliteArticleRepository>();
            services.AddTransient<IUserRepository, SqliteUserRepository>();
            services.AddTransient<ITagRepository, SqliteTagRepository>();
            services.AddTransient<ISocialService, SocialService>();
        }
    }
}