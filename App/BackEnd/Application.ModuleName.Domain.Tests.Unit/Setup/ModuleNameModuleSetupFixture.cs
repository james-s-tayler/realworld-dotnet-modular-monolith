using System.Collections.Generic;
using Application.Core.Modules;
using Application.Core.Testing;
using Application.ModuleName.Domain.Setup.Module;
using Microsoft.Extensions.DependencyInjection;

namespace Application.ModuleName.Domain.Tests.Unit.Setup
{
    public class ModuleNameModuleSetupFixture : AbstractModuleSetupFixture
    {
        protected override void AddConfiguration(IDictionary<string, string> configuration)
        {
            
        }
        
        protected override void ReplaceServices(AbstractModule module)
        {
            
        }
        
        protected override void SetupPostProcess(ServiceProvider provider)
        {
            
        }

        public override void PerTestSetup()
        {
            
        }

        public ModuleNameModuleSetupFixture() : base(new ModuleNameModule()) {}
    }
}