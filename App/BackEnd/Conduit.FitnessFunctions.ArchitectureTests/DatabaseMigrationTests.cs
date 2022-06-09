using System.Linq;
using ArchUnitNET.Domain.Dependencies;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent.Conditions;
using ArchUnitNET.xUnit;
using JetBrains.Annotations;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace Conduit.FitnessFunctions.ArchitectureTests
{
    [Collection(nameof(ArchitectureTestCollection))]
    public class DatabaseMigrationTests
    {
        private readonly ArchitectureTestSetupFixture _application;

        public DatabaseMigrationTests(ArchitectureTestSetupFixture application)
        {
            _application = application;
        }

        [Fact]
        public void DatabaseMigrationsStartWithModuleName()
        {
            Classes().That().Are(_application.DatabaseMigrations)
                .Should()
                .FollowCustomCondition(databaseMigration =>
                {
                    var @namespace = databaseMigration.Namespace.FullName.Split(".");
                    var moduleName = @namespace[1];
                    var dbMigrationPrefix = $"{moduleName}Domain_"; //e.g Conduit.Users.Domain.Migrations => UsersDomain_

                    var pass = databaseMigration.NameStartsWith(dbMigrationPrefix);
                    
                    return new ConditionResult(databaseMigration, pass, $"does not start with {moduleName}Domain_");
                }, "start with {moduleName}Domain_")
                .Because("FluentMigrator console logger needs some way to help us differentiate what database a migration was run on.")
                .Check(_application.Architecture);
        }
    }
}