using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Xunit.Abstractions;

namespace Application.Core.Testing
{
    public abstract class TestBase
    {
        protected TestBase(ITestOutputHelper testOutputHelper)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Is(LogEventLevel.Debug)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .Enrich.WithCorrelationId()
                .Enrich.WithExceptionDetails()
                .Destructure.ToMaximumDepth(5)
                .Destructure.ToMaximumCollectionCount(100)
                .Destructure.ToMaximumStringLength(10000)
                .WriteTo.TestOutput(testOutputHelper, LogEventLevel.Debug)
                .CreateLogger();
        }
    }
}