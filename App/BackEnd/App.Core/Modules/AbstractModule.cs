using System;
using System.Diagnostics;
using System.Reflection;
using App.Core.Context;
using App.Core.Logging;
using App.Core.PipelineBehaviors.Authorization;
using App.Core.PipelineBehaviors.Events;
using App.Core.PipelineBehaviors.Logging;
using App.Core.PipelineBehaviors.Transactions;
using App.Core.PipelineBehaviors.Validation;
using App.Core.SchemaManagement;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Serilog;
using AppContext = App.Core.Context.AppContext;

namespace App.Core.Modules
{
    public abstract class AbstractModule : IHostingStartup, IModule
    {
        private IServiceCollection _services;

        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                InitializeModule(context.Configuration, context.HostingEnvironment, services);
                Log.Information("Registered Module: {0} in {1}ms", GetModuleName(), stopWatch.ElapsedMilliseconds);
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
            //dubious about web specific stuff leaking into here but will forgo too much speculative generality 
            services.AddHttpContextAccessor();
            services.TryAddTransient<IRequestClaimsPrincipalProvider, HttpContextRequestClaimsPrincipalProvider>();
            services.TryAddTransient<IRequestAuthorizationProvider, HttpContextRequestAuthorizationProvider>();

            services.TryAddTransient<IUserContext, AppContext>();
            services.TryAddTransient<UserContextEnricher>();
            services.TryAddTransient<IDataAnnotationsValidator, DataAnnotationsValidator>();
            services.TryAddTransient<IInputContractValidator, InputContractValidator>();
            services.AddMediatR(GetModuleAssembly());
            services.AddValidatorsFromAssembly(GetModuleAssembly(), ServiceLifetime.Transient, null, true);
            services.AddAuthorizersFromAssembly(GetModuleAssembly(), ServiceLifetime.Transient);

            services.AddPipelineBehaviorsFromAssembly(GetModuleContractsAssembly(), typeof(OperationLoggingPipelineBehavior<,>));
            services.AddPipelineBehaviorsFromAssembly(GetModuleContractsAssembly(), typeof(EventPublishingPipelineBehavior<,>));
            services.AddTransactionPipelineBehaviorsFromAssembly(GetModuleContractsAssembly(), GetModuleType());
            services.AddPipelineBehaviorsFromAssembly(GetModuleContractsAssembly(), typeof(AuthorizationPipelineBehavior<,>));
            services.AddPipelineBehaviorsFromAssembly(GetModuleContractsAssembly(), typeof(ContractValidationPipelineBehavior<,>));
            services.AddPipelineBehaviorsFromAssembly(GetModuleContractsAssembly(), typeof(BusinessValidationPipelineBehavior<,>));
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