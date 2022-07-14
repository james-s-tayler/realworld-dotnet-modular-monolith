using System.Collections.Generic;
using App.Core.Modules;
using App.Core.Testing;
using App.Feed.Domain.Setup.Module;
using Microsoft.Extensions.DependencyInjection;

namespace App.Feed.Domain.Tests.Unit.Setup
{
    public class FeedModuleSetupFixture : AbstractModuleSetupFixture
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

        public FeedModuleSetupFixture() : base(new FeedModule()) {}
    }
}