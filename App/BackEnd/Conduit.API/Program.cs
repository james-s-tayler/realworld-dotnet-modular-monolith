using Conduit.Identity.Domain;
using Conduit.Identity.Domain.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Conduit.API
{
    /// <summary>
    /// Program
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Create the host builder.
        /// </summary>
        /// <param name="args"></param>
        /// <returns>IHostBuilder</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseSetting(WebHostDefaults.HostingStartupAssembliesKey,
                        $"{IdentityDomain.Assembly.GetName().Name}");
                   webBuilder.UseStartup<Startup>();
                });
    }
}
