using Xunit;

namespace App.Social.Domain.Tests.Unit.Setup
{
    [CollectionDefinition(nameof(SocialModuleTestCollection))]
    public class SocialModuleTestCollection : ICollectionFixture<SocialModuleSetupFixture>
    {
        
    }
}