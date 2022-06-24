using System.Collections.Generic;
using Application.Content.Domain.Infrastructure.Repositories;
using Application.Core.Modules;
using Application.Core.Testing;
using Application.Content.Domain.Setup.Module;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Content.Domain.Tests.Unit.Setup
{
    public class ContentModuleSetupFixture : AbstractModuleSetupFixture
    {
        internal IUserRepository UserRepository { get; private set; }
        
        protected override void AddConfiguration(IDictionary<string, string> configuration)
        {
        }

        protected override void ReplaceServices(AbstractModule module)
        {
        }

        protected override void SetupPostProcess(ServiceProvider provider)
        {
            UserRepository = provider.GetService<IUserRepository>();
        }

        public ContentModuleSetupFixture() : base(new ContentModule())
        {
        }
    }
}