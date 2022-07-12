using Xunit;

namespace Application.Feed.Domain.Tests.Unit.Setup
{
    [CollectionDefinition(nameof(FeedModuleTestCollection))]
    public class FeedModuleTestCollection : ICollectionFixture<FeedModuleSetupFixture>
    {
        
    }
}