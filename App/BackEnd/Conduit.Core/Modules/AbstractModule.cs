using System;
using System.Reflection;
using Conduit.Core.Context;
using Conduit.Core.PipelineBehaviors.Authorization;
using Conduit.Core.PipelineBehaviors.Logging;
using Conduit.Core.PipelineBehaviors.Validation;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Conduit.Core.Modules
{
    public abstract class AbstractModule : IHostingStartup
    {
        private IServiceCollection _services;
        
        public void Configure(IWebHostBuilder builder)
        {
            Console.WriteLine($"Registering Module: {GetModuleAssembly().GetName().Name}");
            builder.ConfigureServices((context, services) =>
            {
                AddServices(context.Configuration, services);
            });
        }

        public void AddServices(IConfiguration configuration, IServiceCollection services)
        {
            _services = services;
            AddModuleUseCases(_services);
            AddModuleServices(configuration, _services);
        }

        private void AddModuleUseCases(IServiceCollection services)
        {
            services.AddHttpContextAccessor(); //dubious about web specific assemblies leaking into here...
            services.TryAddScoped<IUserContext, ApiContext>();
            services.AddMediatR(GetModuleAssembly());
            services.AddValidatorsFromAssembly(GetModuleAssembly());
            services.AddAuthorizersFromAssembly(GetModuleAssembly());
            
            services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(OperationLoggingPipelineBehavior<,>));
            //add telemetry pipeline behavior
            services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(AuthorizationPipelineBehavior<,>));
            //add transaction pipeline behavior
            services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
        }

        protected abstract Assembly GetModuleAssembly();
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