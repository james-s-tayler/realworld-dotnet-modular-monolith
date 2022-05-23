using ArchUnitNET.xUnit;
using Conduit.Core.DataAccess;
using FluentAssertions;
using JetBrains.Annotations;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace Conduit.FitnessFunctions.ArchitectureTests
{
    [Collection(nameof(ArchitectureTestCollection))]
    public class DomainContractTests
    {
        private readonly ArchitectureTestSetupFixture _conduit;

        public DomainContractTests([NotNull] ArchitectureTestSetupFixture conduit)
        {
            _conduit = conduit;
        }
        
        [Fact]
        public void ArchitectureHasDomainContracts()
        {
            _conduit.DomainContractClasses.GetObjects(_conduit.Architecture).Should().NotBeEmpty();
        }
        
        [Fact]
        public void DomainContractsMustSubclassContractModel()
        {
            Classes().That().Are(_conduit.DomainContractClasses)
                .Should().BeAssignableTo(typeof(ContractModel))
                .Because("Fody.Tracer [NoTrace] is applied to ContractModel.ToString() to reduce log noise")
                .Check(_conduit.Architecture);
        }
    }
}