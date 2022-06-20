using Xunit;

namespace Application.Content.Domain.Tests.Unit.Setup
{
    [CollectionDefinition(nameof(ContentTestCollection))]
    public class ContentTestCollection : ICollectionFixture<ContentSetupFixture>
    {
    }
}