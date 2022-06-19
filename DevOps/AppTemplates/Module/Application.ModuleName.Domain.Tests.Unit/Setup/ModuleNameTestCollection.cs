using Xunit;

namespace Application.ModuleName.Domain.Tests.Unit.Setup
{
    [CollectionDefinition(nameof(ModuleNameTestCollection))]
    public class ModuleNameTestCollection : ICollectionFixture<ModuleNameSetupFixture>
    {
        
    }
}