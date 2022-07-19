using System.Diagnostics;
using JetBrains.Annotations;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Xunit.Abstractions;

namespace App.Core.Testing
{
    public abstract class UnitTestBase
    {
        protected UnitTestBase([NotNull] ITestOutputHelper testOutputHelper, [NotNull] AbstractModuleSetupFixture module)
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
            
            using var transaction = module.ModuleDbConnection.Connection.BeginTransaction();
            
            var clearModuleDatabaseTablesStopwatch = new Stopwatch();
            clearModuleDatabaseTablesStopwatch.Start();
            module.ClearModuleDatabaseTables();
            Log.Information("ClearModuleDatabaseTables elapsed: {0}ms", clearModuleDatabaseTablesStopwatch.ElapsedMilliseconds);
            
            var defaultUserContextStopwatch = new Stopwatch();
            defaultUserContextStopwatch.Start();
            module.SetDefaultUserContext();
            Log.Information("SetDefaultUserContext elapsed: {0}ms", defaultUserContextStopwatch.ElapsedMilliseconds);
            
            var perTestSetupstopwatch = new Stopwatch();
            perTestSetupstopwatch.Start();
            module.PerTestSetup();
            Log.Information("PerTestSetup elapsed: {0}ms", perTestSetupstopwatch.ElapsedMilliseconds);
            
            transaction.Commit();
        }
    }
}