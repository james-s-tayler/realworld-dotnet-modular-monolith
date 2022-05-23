using System.Reflection;
using System.Runtime.CompilerServices;
using Conduit.Core.Modules;
using Conduit.Users.Domain;
using Conduit.Users.Domain.Configuration;
using Conduit.Users.Domain.Entities;
using Conduit.Users.Domain.Infrastructure.Repositories;
using Conduit.Users.Domain.Infrastructure.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ScottBrady91.AspNetCore.Identity;

//would be epic to add this in via Fody!
[assembly: HostingStartup(typeof(UsersModule))]
[assembly: InternalsVisibleTo("Conduit.Users.Domain.Tests.Unit")]
namespace Conduit.Users.Domain
{
    internal class UsersModule : AbstractModule
    {
        protected override Assembly GetModuleAssembly()
        {
            return UsersDomain.Assembly;
        }

        protected override void AddModuleServices(IConfiguration configuration, IServiceCollection services)
        {
            services.TryAddSingleton<IPasswordHasher<User>, BCryptPasswordHasher<User>>();
            services.AddSingleton<IUserRepository, InMemoryUserRepository>();
            services.AddSingleton<IAuthTokenService, JwtAuthTokenService>();
            services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));
        }
    }
}