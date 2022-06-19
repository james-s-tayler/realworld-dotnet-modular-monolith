using System.Reflection;
using System.Runtime.CompilerServices;
using Application.Core.DataAccess.Dapper.Sqlite;
using Application.Core.Modules;
using Application.Core.SchemaManagement;
using Application.Users.Domain.Contracts;
using Application.Users.Domain.Entities;
using Application.Users.Domain.Infrastructure.Repositories;
using Application.Users.Domain.Infrastructure.Services;
using Application.Users.Domain.Setup.Configuration;
using Application.Users.Domain.Setup.Module;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using ScottBrady91.AspNetCore.Identity;

//would be epic to add this in via Fody!
[assembly: HostingStartup(typeof(UsersModule))]
[assembly: InternalsVisibleTo("Application.Users.Domain.Tests.Unit")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace Application.Users.Domain.Setup.Module
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
            services.TryAddTransient<IPasswordHasher<User>, BCryptPasswordHasher<User>>();
            services.AddTransient<IUserRepository, SqliteUserRepository>();
            services.AddTransient<IAuthTokenService, JwtAuthTokenService>();
            services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));
        }
    }
}