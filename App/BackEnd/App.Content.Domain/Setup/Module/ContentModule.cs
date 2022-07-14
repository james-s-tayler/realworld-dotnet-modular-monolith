using System.Reflection;
using System.Runtime.CompilerServices;
using App.Content.Domain.Contracts;
using App.Content.Domain.Infrastructure.Repositories;
using App.Content.Domain.Infrastructure.Services;
using App.Content.Domain.Setup.Module;
using App.Core.DataAccess.Dapper.Sqlite;
using App.Core.Modules;
using App.Core.SchemaManagement;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

//would be epic to add this in via Fody!
[assembly: HostingStartup(typeof(ContentModule))]
[assembly: InternalsVisibleTo("App.Content.Domain.Tests.Unit")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace App.Content.Domain.Setup.Module
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
            services.AddTransient<ICommentRepository, SqliteCommentRepository>();
            services.AddTransient<IArticleRepository, SqliteArticleRepository>();
            services.AddTransient<IUserRepository, SqliteUserRepository>();
            services.AddTransient<ITagRepository, SqliteTagRepository>();
            services.AddTransient<ISocialService, SocialService>();
        }
    }
}