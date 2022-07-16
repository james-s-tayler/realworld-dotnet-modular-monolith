using Xunit;

namespace App.FitnessFunctions.ArchitectureTests
{
    [CollectionDefinition(nameof(ArchitectureTestCollection))]
    public class ArchitectureTestCollection : ICollectionFixture<ArchitectureTestSetupFixture>
    {
    }
}