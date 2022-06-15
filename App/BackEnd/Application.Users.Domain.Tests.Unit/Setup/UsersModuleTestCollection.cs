using Xunit;

namespace Application.Users.Domain.Tests.Unit.Setup
{
    [CollectionDefinition(nameof(UsersModuleTestCollection))]
    public class UsersModuleTestCollection : ICollectionFixture<UsersModuleSetupFixture>
    {
        
    }
}