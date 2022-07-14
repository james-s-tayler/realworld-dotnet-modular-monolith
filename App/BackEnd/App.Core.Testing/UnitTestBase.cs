﻿using JetBrains.Annotations;
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
            
            module.ClearModuleDatabaseTables();
            module.SetDefaultUserContext();
            module.PerTestSetup();
            
            transaction.Commit();
        }
    }
}