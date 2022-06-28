using Xunit;

namespace Application.Content.Domain.Tests.Unit.Setup
{
    [CollectionDefinition(nameof(ContentModuleTestCollection))]
    public class ContentModuleTestCollection : ICollectionFixture<ContentModuleSetupFixture>
    {
    }
}