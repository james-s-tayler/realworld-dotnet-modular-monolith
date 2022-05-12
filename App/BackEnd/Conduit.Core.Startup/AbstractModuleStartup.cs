using System;
using System.Reflection;
using Conduit.Core.Validation;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Conduit.Core.Startup
{
    public abstract class AbstractModuleStartup : IHostingStartup
    {
        private IServiceCollection _services;
        
        public void Configure(IWebHostBuilder builder)
        {
            Console.WriteLine($"Registering Module: {GetModuleAssembly().GetName().Name}");
            builder.ConfigureServices(AddServices);
        }

        public void AddServices(IServiceCollection services)
        {
            _services = services;
            AddModuleUseCases(_services);
            AddModuleServices(_services);
        }

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

        private void AddModuleUseCases(IServiceCollection services)
        {
            services.AddMediatR(GetModuleAssembly());
            services.AddValidatorsFromAssembly(GetModuleAssembly());
            services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(CommandValidationPipelineBehavior<,>));
        }

        protected abstract Assembly GetModuleAssembly();
        protected abstract void AddModuleServices(IServiceCollection services);
    }
}