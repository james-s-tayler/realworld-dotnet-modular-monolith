using Xunit;

namespace Conduit.Social.Domain.Tests.Unit.Setup
{
    [CollectionDefinition(nameof(SocialModuleTestCollection))]
    public class SocialModuleTestCollection : ICollectionFixture<SocialModuleSetupFixture>
    {
        
    }
}