using System;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Formatting.Compact;

namespace Conduit.API
{
    /// <summary>
    /// Program
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            var assembly = Assembly.GetExecutingAssembly().GetName();
            var version = Environment.GetEnvironmentVariable("GIT_COMMIT_HASH") ?? assembly.Version!.ToString(); 
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Is(env.Equals("Development") ? LogEventLevel.Debug : LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                //add standard metadata to all log entries
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Service.Name", $"{assembly.Name}")
                .Enrich.WithProperty("Service.Version", version)
                .Enrich.WithCorrelationId()
                .Enrich.WithExceptionDetails()
                //put safety limits on destructuring objects when using the @ destructuring operator
                .Destructure.ToMaximumDepth(5)
                .Destructure.ToMaximumCollectionCount(100)
                .Destructure.ToMaximumStringLength(10000)
                //output logs to console and Seq
                .WriteTo.Console(new RenderedCompactJsonFormatter())
                .WriteTo.Seq(Environment.GetEnvironmentVariable("SEQ_URL") ?? "http://localhost:5341")
                .CreateLogger();
            
            try
            {
                CreateHostBuilder(args)
                    .Build()
                    //.MigrateDatabase()
                    .Run();
            }
            catch (Exception e)
            {
                Log.Error(e, e.Message);
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseSetting(WebHostDefaults.HostingStartupAssembliesKey, "Conduit.Users.Domain");
                    webBuilder.UseStartup<Startup>();
                })
                .UseSerilog();
    }
}
