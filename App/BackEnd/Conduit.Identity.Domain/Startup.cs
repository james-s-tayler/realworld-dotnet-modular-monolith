using System;
using Conduit.Core.Validation;
using Conduit.Identity.Domain.Entities;
using Conduit.Identity.Domain.Interactions.Outbound.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ScottBrady91.AspNetCore.Identity;

[assembly: HostingStartup(typeof(Conduit.Identity.Domain.Startup))]

namespace Conduit.Identity.Domain
{
    internal class Startup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            Console.WriteLine($"Registering Module: {IdentityDomain.AssemblyName}...");
            
            builder.ConfigureServices(services =>
            {
                services.AddScoped<IUserRepository, InMemoryUserRepository>();
                services.TryAddSingleton<IPasswordHasher<User>, BCryptPasswordHasher<User>>();
                
                services.AddMediatR(IdentityDomain.Assembly);
                services.AddValidatorsFromAssembly(IdentityDomain.Assembly);
                services.TryAddSingleton(typeof(IPipelineBehavior<,>), typeof(CommandValidationPipelineBehavior<,>));
            });
        }
    }
}