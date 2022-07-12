using System.Collections.Generic;
using Application.Core.Modules;
using Application.Core.Testing;
using Application.Feed.Domain.Setup.Module;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Feed.Domain.Tests.Unit.Setup
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