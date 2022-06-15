using System;
using System.Reflection;
using Application.Core.Context;
using Application.Core.Logging;
using Application.Core.PipelineBehaviors.Authorization;
using Application.Core.PipelineBehaviors.Events;
using Application.Core.PipelineBehaviors.Logging;
using Application.Core.PipelineBehaviors.Transactions;
using Application.Core.PipelineBehaviors.Validation;
using Application.Core.SchemaManagement;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace Application.Core.Modules
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

        public void InitializeModule(IConfiguration configuration, IHostEnvironment hostEnvironment,
            IServiceCollection services)
        {
            AddServices(configuration, services);
            AddModuleDatabase(configuration, hostEnvironment, services);
        }

        private void AddModuleDatabase(IConfiguration configuration, IHostEnvironment hostEnvironment,
            IServiceCollection services)
        {
            AddDbConnectionFactory(services, configuration, hostEnvironment);
            RunModuleDatabaseMigrations(new SchemaManager(configuration, hostEnvironment, GetModuleAssembly()));
        }

        protected abstract void AddDbConnectionFactory(IServiceCollection services, IConfiguration configuration,
            IHostEnvironment hostEnvironment);

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

            services.AddPipelineBehaviorsFromAssembly(GetModuleContractsAssembly(), typeof(OperationLoggingPipelineBehavior<,>));
            services.AddPipelineBehaviorsFromAssembly(GetModuleContractsAssembly(), typeof(EventPublishingPipelineBehavior<,>));
            services.AddTransactionPipelineBehaviorsFromAssembly(GetModuleContractsAssembly(), GetModuleType());
            services.AddPipelineBehaviorsFromAssembly(GetModuleContractsAssembly(), typeof(AuthorizationPipelineBehavior<,>));
            services.AddPipelineBehaviorsFromAssembly(GetModuleContractsAssembly(), typeof(ValidationPipelineBehavior<,>));
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