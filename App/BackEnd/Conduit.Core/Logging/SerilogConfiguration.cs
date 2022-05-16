using System;
using System.Diagnostics;
using System.Reflection;
using Destructurama;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Formatting.Compact;

namespace Conduit.Core.Logging
{
    public static class SerilogConfiguration
    {
        public static void SetupSerilog()
        {
            //https://benfoster.io/blog/serilog-best-practices/
            
            var callerAssembly = new StackFrame(1).GetMethod().DeclaringType.Assembly.GetName();
            var version = Environment.GetEnvironmentVariable("GIT_COMMIT_HASH") ?? callerAssembly.Version!.ToString(); 
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
            
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Is(env.Equals("Development") || env.Equals("Docker") ? LogEventLevel.Debug : LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                //add standard metadata to all log entries
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Service.Name", callerAssembly.Name)
                .Enrich.WithProperty("Service.Version", version)
                .Enrich.WithCorrelationId()
                .Enrich.WithExceptionDetails()
                //put safety limits on destructuring objects when using the @ destructuring operator
                .Destructure.UsingAttributes() //allow [NotLogged]
                .Destructure.ToMaximumDepth(5)
                .Destructure.ToMaximumCollectionCount(100)
                .Destructure.ToMaximumStringLength(10000)
                //output logs to console and Seq
                .WriteTo.Console(new RenderedCompactJsonFormatter())
                .WriteTo.Seq(Environment.GetEnvironmentVariable("SEQ_URL") ?? "http://localhost:5341")
                .CreateLogger();
        }
    }
}