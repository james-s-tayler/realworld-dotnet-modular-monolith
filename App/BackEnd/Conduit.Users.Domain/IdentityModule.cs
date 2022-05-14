using System.Reflection;
using System.Runtime.CompilerServices;
using Conduit.Core.Modules;
using Conduit.Identity.Domain.Configuration;
using Conduit.Identity.Domain.Entities;
using Conduit.Identity.Domain.Infrastructure.Repositories;
using Conduit.Identity.Domain.Infrastructure.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ScottBrady91.AspNetCore.Identity;

//would be epic to add this in via Fody!
[assembly: HostingStartup(typeof(Conduit.Identity.Domain.IdentityModule))]
[assembly: InternalsVisibleTo("Conduit.Users.Domain.Tests.Unit")]
namespace Conduit.Identity.Domain
{
    internal class IdentityModule : AbstractModule
    {
        protected override Assembly GetModuleAssembly()
        {
            return IdentityDomain.Assembly;
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