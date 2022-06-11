using System.Collections.Generic;
using AutoFixture;
using Conduit.Core.Context;
using Conduit.Core.Modules;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using Serilog;

namespace Conduit.Core.Testing
{
    public abstract class AbstractModuleSetupFixture
    {
        public int AuthenticatedUserId { get; private set; }
        public string AuthenticatedUserUsername { get; private set; }
        public string AuthenticatedUserEmail { get; private set; }
        public string AuthenticatedUserToken { get; private set; }
        public Fixture AutoFixture { get; } = new ();
        public IMediator Mediator { get; set; }
        public AbstractModule Module { get; }
        public IServiceCollection Services { get; }
        public ConfigurationBuilder Configuration { get; }
        public Mock<IHostEnvironment> _hostEnvironment;
        
        public Mock<IUserContext> UserContext { get; } = new ();
        
        public AbstractModuleSetupFixture(AbstractModule module)
        {
            Module = module;
            _hostEnvironment = new Mock<IHostEnvironment>();
            _hostEnvironment.Setup(environment => environment.EnvironmentName).Returns("Test");
            Services = new ServiceCollection();
            Configuration = new ConfigurationBuilder();
            var configuration = new Dictionary<string, string>
            {
                {$"DatabaseConfig:{Module.GetModuleName()}:DatabaseName", GetDatabaseName()}
            };
            
            AddConfiguration(configuration);
            Configuration.AddInMemoryCollection(configuration);
            
            WithRandomUserContext();

            Services.AddLogging(builder =>
            {
                builder.AddSerilog();
                builder.SetMinimumLevel(LogLevel.Debug);
            });
            
            Module.InitializeModule(Configuration.Build(), _hostEnvironment.Object, Services);
            Module.ReplaceTransient(UserContext.Object);
            ReplaceServices(Module);

            var provider = Services.BuildServiceProvider();
            Mediator = provider.GetRequiredService<IMediator>();
            SetupPostProcess(provider);
        }
        
        protected abstract void AddConfiguration(IDictionary<string, string> configuration);

        protected abstract void ReplaceServices(AbstractModule module);
        
        protected abstract void SetupPostProcess(ServiceProvider provider);

        public string GetDatabaseName()
        {
            return Module.GetType().Name.Replace("Module", "").ToLowerInvariant();
        }

        public void WithUnauthenticatedUserContext()
        {
            UserContext.Reset();
            UserContext.SetupGet(context => context.IsAuthenticated).Returns(false);
        }

        public void WithRandomUserContext()
        {
            AuthenticatedUserId = AutoFixture.Create<int>();
            AuthenticatedUserUsername = AutoFixture.Create<string>();
            AuthenticatedUserEmail = $"{AuthenticatedUserUsername}@{AutoFixture.Create<string>()}.com";
            AuthenticatedUserToken = AutoFixture.Create<string>();
            WithUserContextReturning(true, AuthenticatedUserId, AuthenticatedUserUsername, AuthenticatedUserEmail, AuthenticatedUserToken);
        }
        
        public void WithUserContextReturning(bool isAuthenticated, int userId, string username, string email, string token)
        {
            UserContext.Reset();
            UserContext.SetupGet(context => context.IsAuthenticated).Returns(isAuthenticated);
            UserContext.SetupGet(context => context.UserId).Returns(userId);
            UserContext.SetupGet(context => context.Username).Returns(username);
            UserContext.SetupGet(context => context.Email).Returns(email);
            UserContext.SetupGet(context => context.Token).Returns(token);
        }
    }
}