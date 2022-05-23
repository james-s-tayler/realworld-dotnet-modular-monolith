using FluentAssertions;
using Xunit;

namespace Conduit.FitnessFunctions.ArchitectureTests
{
    [Collection(nameof(ArchitectureTestCollection))]
    public class SanityChecks
    {
        private readonly ArchitectureTestSetupFixture _conduit;

        public SanityChecks(ArchitectureTestSetupFixture conduit)
        {
            _conduit = conduit;
        }

        [Fact]
        public void ArchitectureHasDomainContracts()
        {
            _conduit.DomainContractClasses.GetObjects(_conduit.Architecture).Should().NotBeEmpty();
        }
        
        [Fact]
        public void ArchitectureHasCommands()
        {
            _conduit.Commands.GetObjects(_conduit.Architecture).Should().NotBeEmpty();
        }
        
        [Fact]
        public void ArchitectureHasQueries()
        {
            _conduit.Queries.GetObjects(_conduit.Architecture).Should().NotBeEmpty();
        }
    }
}