using Xunit;

namespace Application.FitnessFunctions.ArchitectureTests
{
    [CollectionDefinition(nameof(ArchitectureTestCollection))]
    public class ArchitectureTestCollection : ICollectionFixture<ArchitectureTestSetupFixture>
    {
    }
}