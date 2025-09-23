using System.Reflection;
using App.Core.PipelineBehaviors.Authorization;
using App.Core.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace App.Core.Modules
{
    // from: https://medium.com/@austin.davies0101/creating-a-basic-authorization-pipeline-with-mediatr-and-asp-net-core-c257fe3cc76b
    public static class ServiceCollectionExtensions
    {
        public static void AddAuthorizersFromAssembly(
            this IServiceCollection services,
            Assembly assembly,
            ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            var authorizerType = typeof(IAuthorizer<>);
            assembly.GetTypesAssignableTo(authorizerType).ForEach((type) =>
            {
                foreach ( var implementedInterface in type.ImplementedInterfaces )
                {
                    switch ( lifetime )
                    {
                        case ServiceLifetime.Scoped:
                            services.AddScoped(implementedInterface, type);
                            break;
                        case ServiceLifetime.Singleton:
                            services.AddSingleton(implementedInterface, type);
                            break;
                        case ServiceLifetime.Transient:
                            services.AddTransient(implementedInterface, type);
                            break;
                    }
                }
            });
        }
    }
}