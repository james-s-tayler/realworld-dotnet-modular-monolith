using Xunit;

namespace App.Feed.Domain.Tests.Unit.Setup
{
    [CollectionDefinition(nameof(FeedModuleTestCollection))]
    public class FeedModuleTestCollection : ICollectionFixture<FeedModuleSetupFixture>
    {

    }
}