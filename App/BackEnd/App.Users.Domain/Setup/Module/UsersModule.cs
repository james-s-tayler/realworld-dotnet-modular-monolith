using System.Reflection;
using System.Runtime.CompilerServices;
using App.Core.DataAccess.Dapper.Sqlite;
using App.Core.Modules;
using App.Core.SchemaManagement;
using App.Users.Domain.Contracts;
using App.Users.Domain.Entities;
using App.Users.Domain.Infrastructure.Repositories;
using App.Users.Domain.Infrastructure.Services;
using App.Users.Domain.Setup.Configuration;
using App.Users.Domain.Setup.Module;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using ScottBrady91.AspNetCore.Identity;

//would be epic to add this in via Fody!
[assembly: HostingStartup(typeof(UsersModule))]
[assembly: InternalsVisibleTo("App.Users.Domain.Tests.Unit")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace App.Users.Domain.Setup.Module
{
    internal class UsersModule : AbstractModule
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
            return typeof(UsersModule).Assembly;
        }

        public override Assembly GetModuleContractsAssembly()
        {
            return UsersDomainContracts.Assembly;
        }

        protected override void AddModuleServices(IConfiguration configuration, IServiceCollection services)
        {
            services.TryAddTransient<IPasswordHasher<UserEntity>, BCryptPasswordHasher<UserEntity>>();
            services.AddTransient<IUserRepository, SqliteUserRepository>();
            services.AddTransient<IAuthTokenService, JwtAuthTokenService>();
            services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));
        }
    }
}