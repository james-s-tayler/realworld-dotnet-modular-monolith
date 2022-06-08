using System;
using System.Reflection;
using Conduit.Core.Context;
using Conduit.Core.Logging;
using Conduit.Core.PipelineBehaviors.Authorization;
using Conduit.Core.PipelineBehaviors.Logging;
using Conduit.Core.PipelineBehaviors.Transactions;
using Conduit.Core.PipelineBehaviors.Validation;
using Conduit.Core.SchemaManagement;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace Conduit.Core.Modules
{
    public abstract class AbstractModule : IHostingStartup, IModule
    {
        private IServiceCollection _services;
        
        public void Configure(IWebHostBuilder builder)
        {
            Console.WriteLine($"Registering Module: {GetModuleName()}");
            builder.ConfigureServices((context, services) =>
            {
                InitializeModule(context.Configuration, context.HostingEnvironment, services);
            });
        }

        public void InitializeModule(IConfiguration configuration, IHostEnvironment hostEnvironment, IServiceCollection services)
        {
            AddServices(configuration, services);
            AddModuleDatabase(configuration, hostEnvironment, services);
        }

        private void AddModuleDatabase(IConfiguration configuration, IHostEnvironment hostEnvironment, IServiceCollection services)
        {
            AddDbConnectionFactory(services, configuration, hostEnvironment);
            RunModuleDatabaseMigrations(new SchemaManager(configuration, hostEnvironment, GetModuleAssembly()));
        }

        protected abstract void AddDbConnectionFactory(IServiceCollection services, IConfiguration configuration, IHostEnvironment hostEnvironment);
        protected abstract void RunModuleDatabaseMigrations(SchemaManager schemaManager);

        public void AddServices(IConfiguration configuration, IServiceCollection services)
        {
            _services = services;
            AddModuleUseCases(_services);
            AddModuleServices(configuration, _services);
        }

        private void AddModuleUseCases(IServiceCollection services)
        {
            services.AddHttpContextAccessor(); //dubious about web specific assemblies leaking into here...
            services.TryAddTransient<IUserContext, ApiContext>();
            services.TryAddTransient<UserContextEnricher>();
            services.AddMediatR(GetModuleAssembly());
            services.AddValidatorsFromAssembly(GetModuleAssembly(), ServiceLifetime.Transient, null, true);
            services.AddAuthorizersFromAssembly(GetModuleAssembly(), ServiceLifetime.Transient);
            
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(OperationLoggingPipelineBehavior<,>));
            services.AddTransactionPipelineBehaviorsFromAssembly(GetModuleContractsAssembly(), GetModuleType());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationPipelineBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
        }

        public abstract Assembly GetModuleAssembly();
        public abstract Assembly GetModuleContractsAssembly();

        public string GetModuleName()
        {
            return GetModuleAssembly().GetName().Name;
        }

        public Type GetModuleType()
        {
            return GetType();
        }

        protected abstract void AddModuleServices(IConfiguration configuration, IServiceCollection services);
        
        public void ReplaceSingleton<TImplementation>(TImplementation implementation) where TImplementation : class
        {
            _services.Replace(ServiceDescriptor.Singleton(_ => implementation));
        }

        public void ReplaceSingleton<TInterface, TImplementation>(TImplementation implementation) where TImplementation : class, TInterface where TInterface : class
        {
            _services.Replace(ServiceDescriptor.Singleton<TInterface, TImplementation>(_ => implementation));
        }
        
        public void ReplaceScoped<TImplementation>(TImplementation implementation) where TImplementation : class
        {
            _services.Replace(ServiceDescriptor.Scoped(_ => implementation));
        }

        public void ReplaceScoped<TInterface, TImplementation>(TImplementation implementation) where TImplementation : class, TInterface where TInterface : class
        {
            _services.Replace(ServiceDescriptor.Scoped<TInterface, TImplementation>(_ => implementation));
        }
        
        public void ReplaceTransient<TImplementation>(TImplementation implementation) where TImplementation : class
        {
            _services.Replace(ServiceDescriptor.Transient(_ => implementation));
        }

        public void ReplaceTransient<TInterface, TImplementation>(TImplementation implementation) where TImplementation : class, TInterface where TInterface : class
        {
            _services.Replace(ServiceDescriptor.Transient<TInterface, TImplementation>(_ => implementation));
        }
    }
}