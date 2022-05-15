using Conduit.Identity.Domain.Tests.Unit.Setup;
using Xunit;

namespace Conduit.Identity.Domain.Tests.Unit.Setup
{
    [CollectionDefinition(nameof(UsersModuleTestCollection))]
    public class UsersModuleTestCollection : ICollectionFixture<UsersModuleSetupFixture>
    {
        
    }
}