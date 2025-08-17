using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using App.Core.Logging;
using App.Core.PipelineBehaviors.Validation;
using FluentValidation;
using FluentValidation.Internal;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Conduit.API
{
    /// <summary>
    /// Program
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Program
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            FluentValidationCustomization.ConfigureFluentValidation();

            try
            {
                CreateHostBuilder(args)
                    .Build()
                    .Run();
            }
            catch ( Exception e )
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseSetting(WebHostDefaults.HostingStartupAssembliesKey,
                        "App.Users.Domain;" +
                        "App.Content.Domain;" +
                        "App.Feed.Domain");
                    webBuilder.UseStartup<Startup>();
                })
                .UseSerilog((context, services, configuration) =>
                {
                    configuration.SetupSerilog(services);
                });
    }
}
