using Conduit.Core.PipelineBehaviors.Transactions;
using Conduit.Users.Domain.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Conduit.Users.Domain.Tests.Unit
{
    public class Test
    {
        [Fact]
        public void Yolo()
        {
            var services = new ServiceCollection();
            services.AddTransactionPipelineBehaviorsFromAssembly(UsersDomainContracts.Assembly, typeof(UsersModule));
        }
    }
}