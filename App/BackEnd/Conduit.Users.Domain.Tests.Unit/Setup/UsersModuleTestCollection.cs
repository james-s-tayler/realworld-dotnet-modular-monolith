using Xunit;

namespace Conduit.Users.Domain.Tests.Unit.Setup
{
    [CollectionDefinition(nameof(UsersModuleTestCollection))]
    public class UsersModuleTestCollection : ICollectionFixture<UsersModuleSetupFixture>
    {
        
    }
}