using Xunit;

namespace Application.Social.Domain.Tests.Unit.Setup
{
    [CollectionDefinition(nameof(SocialModuleTestCollection))]
    public class SocialModuleTestCollection : ICollectionFixture<SocialModuleSetupFixture>
    {
        
    }
}