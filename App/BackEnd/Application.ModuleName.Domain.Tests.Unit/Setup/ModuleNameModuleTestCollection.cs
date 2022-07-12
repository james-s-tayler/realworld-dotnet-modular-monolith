using Xunit;

namespace Application.ModuleName.Domain.Tests.Unit.Setup
{
    [CollectionDefinition(nameof(ModuleNameModuleTestCollection))]
    public class ModuleNameModuleTestCollection : ICollectionFixture<ModuleNameModuleSetupFixture>
    {
        
    }
}