using Xunit;

namespace App.Content.Domain.Tests.Unit.Setup
{
    [CollectionDefinition(nameof(ContentModuleTestCollection))]
    public class ContentModuleTestCollection : ICollectionFixture<ContentModuleSetupFixture>
    {
    }
}