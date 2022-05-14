using System;
using System.Reflection;
using Conduit.Core.Validation.Context;
using Conduit.Core.Validation.CrossCuttingConcerns.Validation;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Conduit.Core.Validation.Modules
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
            services.TryAddScoped<IUserContext, ApiContext>();
            services.AddMediatR(GetModuleAssembly());
            services.AddValidatorsFromAssembly(GetModuleAssembly());
            services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(CommandValidationPipelineBehavior<,>));
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