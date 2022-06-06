using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using Conduit.Core.DataAccess.Dapper.Sqlite;
using Conduit.Core.Modules;
using Conduit.Core.SchemaManagement;
using Conduit.Core.SchemaManagement.Sqlite;
using Conduit.Users.Domain;
using Conduit.Users.Domain.Configuration;
using Conduit.Users.Domain.Entities;
using Conduit.Users.Domain.Infrastructure.Repositories;
using Conduit.Users.Domain.Infrastructure.Services;
using Dapper;
using Dapper.Logging;
using FluentMigrator.Runner.Initialization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using ScottBrady91.AspNetCore.Identity;

//would be epic to add this in via Fody!
[assembly: HostingStartup(typeof(UsersModule))]
[assembly: InternalsVisibleTo("Conduit.Users.Domain.Tests.Unit")]
namespace Conduit.Users.Domain
{
    internal class UsersModule : AbstractModule
    {
        protected override void RunModuleDatabaseMigrations(SchemaManager schemaManager)
        {
            schemaManager.RunSqliteMigrations();
        }

        public override Assembly GetModuleAssembly()
        {
            return UsersDomain.Assembly;
        }

        protected override void AddModuleServices(IConfiguration configuration, IServiceCollection services)
        {
            services.AddTransient<IConnectionStringReader, SqliteConnectionStringReader>();
            services.TryAddTransient<IPasswordHasher<User>, BCryptPasswordHasher<User>>();
            services.AddTransient<IUserRepository, SqliteUserRepository>();
            services.AddTransient<IAuthTokenService, JwtAuthTokenService>();
            services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));
        }
    }
}