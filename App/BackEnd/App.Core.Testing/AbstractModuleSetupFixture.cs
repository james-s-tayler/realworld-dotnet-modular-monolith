using System.Collections.Generic;
using System.Security.Claims;
using App.Core.Context;
using App.Core.DataAccess;
using App.Core.Modules;
using AutoFixture;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using Serilog;

namespace App.Core.Testing
{
    public abstract class AbstractModuleSetupFixture
    {
        public int AuthenticatedUserId { get; private set; }
        public string AuthenticatedUserUsername { get; private set; }
        public string AuthenticatedUserEmail { get; private set; }
        public string AuthenticatedUserToken { get; private set; }
        public string AuthenticatedUserBio { get; private set; }
        public string AuthenticatedUserImage { get; private set; }
        
        public Fixture AutoFixture { get; } = new ();
        public IMediator Mediator { get; }
        public AbstractModule Module { get; }
        public IServiceCollection Services { get; }
        public ConfigurationBuilder Configuration { get; }
        public Mock<IHostEnvironment> _hostEnvironment;
        
        public IUserContext UserContext { get; }
        public Mock<IRequestClaimsPrincipalProvider> RequestClaimsPrincipalProvider { get; } = new();
        public Mock<IRequestAuthorizationProvider> RequestAuthorizationProvider { get; } = new();

        public IModuleDbConnection ModuleDbConnection { get; }
        
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
            
            WithAuthenticatedUserContext();

            Services.AddLogging(builder =>
            {
                builder.AddSerilog();
                builder.SetMinimumLevel(LogLevel.Debug);
            });
            
            Module.InitializeModule(Configuration.Build(), _hostEnvironment.Object, Services);
            Module.ReplaceTransient(RequestClaimsPrincipalProvider.Object);
            Module.ReplaceTransient(RequestAuthorizationProvider.Object);
            ReplaceServices(Module);

            var provider = Services.BuildServiceProvider();
            Mediator = provider.GetRequiredService<IMediator>();
            ModuleDbConnection = provider.GetRequiredService<IModuleDbConnection>();
            UserContext = provider.GetRequiredService<IUserContext>();
            SetupPostProcess(provider);
        }
        
        protected abstract void AddConfiguration(IDictionary<string, string> configuration);

        protected abstract void ReplaceServices(AbstractModule module);
        
        protected abstract void SetupPostProcess(ServiceProvider provider);

        public virtual void ClearModuleDatabaseTables()
        {
            ModuleDbConnection.ClearModuleDatabaseTables();
        }

        public virtual void SetDefaultUserContext()
        {
            WithAuthenticatedUserContext();
        }
        
        public abstract void PerTestSetup();

        public string GetDatabaseName()
        {
            return Module.GetType().Name.Replace("Module", "").ToLowerInvariant();
        }

        public void WithUnauthenticatedUserContext()
        {
            RequestClaimsPrincipalProvider.Reset();
            RequestAuthorizationProvider.Reset();
            RequestClaimsPrincipalProvider.Setup(provider => provider.GetClaimsPrincipal()).Returns(new ClaimsPrincipal());
            RequestAuthorizationProvider.Setup(provider => provider.GetRequestAuthorization()).Returns((string)null);
        }

        public void WithAuthenticatedUserContext()
        {
            AuthenticatedUserId = AutoFixture.Create<int>();
            AuthenticatedUserUsername = AutoFixture.Create<string>();
            AuthenticatedUserEmail = $"{AuthenticatedUserUsername}@{AutoFixture.Create<string>()}.com";
            AuthenticatedUserToken = AutoFixture.Create<string>();
            AuthenticatedUserImage = AutoFixture.Create<string>();
            AuthenticatedUserBio = AutoFixture.Create<string>();
            WithUserContextReturning(AuthenticatedUserId, AuthenticatedUserUsername, AuthenticatedUserEmail, AuthenticatedUserToken);
        }
        
        public void WithUserContextReturning(int userId, string username, string email, string token)
        {
            
            RequestClaimsPrincipalProvider.Reset();
            RequestAuthorizationProvider.Reset();
            
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim("user_id", userId.ToString()),
                new Claim("username", username),
                new Claim("email", email)
            }, "Basic");
      
            var principal = new ClaimsPrincipal(identity);
            
            RequestClaimsPrincipalProvider.Setup(provider => provider.GetClaimsPrincipal()).Returns(principal);
            RequestAuthorizationProvider.Setup(provider => provider.GetRequestAuthorization()).Returns(token);
        }
    }
}