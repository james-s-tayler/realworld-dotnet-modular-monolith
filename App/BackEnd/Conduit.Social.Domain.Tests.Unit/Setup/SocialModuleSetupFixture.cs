using System.Collections.Generic;
using Conduit.Core.Modules;
using Conduit.Core.Testing;
using Conduit.Social.Domain.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Conduit.Social.Domain.Tests.Unit.Setup
{
    public class SocialModuleSetupFixture : AbstractModuleSetupFixture
    {
        internal IUserRepository UserRepository { get; private set; }
        
        public SocialModuleSetupFixture() : base(new SocialModule()) {}

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
    }
}