using Xunit;

namespace Conduit.FitnessFunctions.ArchitectureTests
{
    [CollectionDefinition(nameof(ArchitectureTestCollection))]
    public class ArchitectureTestCollection : ICollectionFixture<ArchitectureTestSetupFixture>
    {
    }
}