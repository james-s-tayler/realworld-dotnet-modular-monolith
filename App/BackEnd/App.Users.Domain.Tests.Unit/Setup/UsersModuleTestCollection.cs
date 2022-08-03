using Xunit;

namespace App.Users.Domain.Tests.Unit.Setup
{
    [CollectionDefinition(nameof(UsersModuleTestCollection))]
    public class UsersModuleTestCollection : ICollectionFixture<UsersModuleSetupFixture>
    {

    }
}