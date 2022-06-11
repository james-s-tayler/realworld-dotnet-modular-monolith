using System.Linq;
using ArchUnitNET.Domain.Dependencies;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent.Conditions;
using ArchUnitNET.xUnit;
using Conduit.Core;
using Conduit.Core.DataAccess;
using JetBrains.Annotations;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace Conduit.FitnessFunctions.ArchitectureTests
{
    [Collection(nameof(ArchitectureTestCollection))]
    public class DomainTests
    {
        private readonly ArchitectureTestSetupFixture _application;

        public DomainTests(ArchitectureTestSetupFixture application)
        {
            _application = application;
        }

        [Fact]
        public void DomainClassesShouldBeInternal()
        {
            Classes().That().Are(_application.DomainClasses)
                .And().AreNot(_application.DomainContractClasses)
                .And().AreNot(_application.DatabaseMigrations)
                .Should()
                .BeInternal()
                .Because("only the contracts of the domain should be public")
                .Check(_application.Architecture);
        }
    }
}