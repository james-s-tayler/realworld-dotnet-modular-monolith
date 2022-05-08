using System;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(Conduit.Identity.Domain.IdentityDomain))]

namespace Conduit.Identity.Domain
{
    public class IdentityDomain : IHostingStartup
    {
        public static readonly Assembly Assembly = typeof(IdentityDomain).Assembly;
        public static readonly string AssemblyName = Assembly.GetName().Name;
        public void Configure(IWebHostBuilder builder)
        {
            Console.WriteLine($"hello from {AssemblyName}!!!!!!!!!!!!!!!!!!!!!!");
            
            /*builder.ConfigureServices(collection =>
            {
                
            });*/
        }
    }
}