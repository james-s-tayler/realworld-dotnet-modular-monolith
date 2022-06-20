using System.Collections.Generic;
using Application.Core.Modules;
using Application.Core.Testing;
using Application.Content.Domain.Setup.Module;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Content.Domain.Tests.Unit.Setup
{
    public class ContentSetupFixture : AbstractModuleSetupFixture
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

        public ContentSetupFixture() : base(new ContentModule())
        {
        }
    }
}