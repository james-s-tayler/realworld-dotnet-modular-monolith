using Xunit;

namespace App.ModuleName.Domain.Tests.Unit.Setup
{
    [CollectionDefinition(nameof(ModuleNameModuleTestCollection))]
    public class ModuleNameModuleTestCollection : ICollectionFixture<ModuleNameModuleSetupFixture>
    {

    }
}