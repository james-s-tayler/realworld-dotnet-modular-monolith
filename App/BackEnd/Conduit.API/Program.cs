using System;
using Application.Core.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Conduit.API
{
    /// <summary>
    /// Program
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                CreateHostBuilder(args)
                    .Build()
                    .Run();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseSetting(WebHostDefaults.HostingStartupAssembliesKey, "Conduit.Users.Domain;Conduit.Social.Domain");
                    webBuilder.UseStartup<Startup>();
                })
                .UseSerilog((context, services, configuration) =>
                {
                    configuration.SetupSerilog(services);
                });
    }
}
