using System;
using System.Data;
using System.Data.Common;
using System.Reflection;
using Conduit.Core.Context;
using Conduit.Core.DataAccess.Dapper.Sqlite;
using Conduit.Core.Logging;
using Conduit.Core.PipelineBehaviors.Authorization;
using Conduit.Core.PipelineBehaviors.Logging;
using Conduit.Core.PipelineBehaviors.Transactions;
using Conduit.Core.PipelineBehaviors.Validation;
using Conduit.Core.SchemaManagement;
using Conduit.Core.SchemaManagement.Sqlite;
using Dapper;
using Dapper.Logging;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

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
            services.AddTransient(_ =>
            {
                var dbConnectionFactory = GetDbConnectionFactory(configuration, hostEnvironment, GetModuleAssembly().GetName().Name);

                if (dbConnectionFactory.GetType() == typeof(SqliteDbConnectionFactory))
                {
                    //this would be problematic if you wanted to have different modules with different database vendors and also using Dapper in both :(
                    if (!SqlMapper.HasTypeHandler(typeof(DateTimeOffset)))
                    {
                        SqlMapper.AddTypeHandler(new SqliteDateTimeOffsetHandler());
                    }
                    if (!SqlMapper.HasTypeHandler(typeof(Guid)))
                    {
                        SqlMapper.AddTypeHandler(new SqliteGuidHandler());
                    }
                    if (!SqlMapper.HasTypeHandler(typeof(TimeSpan)))
                    {
                        SqlMapper.AddTypeHandler(new SqliteTimeSpanHandler());
                    }
                }
                
                return dbConnectionFactory;
            });
            services.AddScoped(provider =>
            {
                var connectionFactory = provider.GetService<IDbConnectionFactory>();
                return connectionFactory.CreateConnection();
            });
            RunModuleDatabaseMigrations(new SchemaManager(configuration, hostEnvironment, GetModuleAssembly()));
        }

        protected abstract IDbConnectionFactory GetDbConnectionFactory(IConfiguration configuration, IHostEnvironment hostEnvironment, string moduleName);
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
            services.AddValidatorsFromAssembly(GetModuleAssembly(), ServiceLifetime.Transient);
            services.AddAuthorizersFromAssembly(GetModuleAssembly(), ServiceLifetime.Transient);
            
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(OperationLoggingPipelineBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionPipelineBehavior<,>));
            //add telemetry pipeline behavior
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationPipelineBehavior<,>));
            //add transaction pipeline behavior
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
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