using System;
using System.Linq;
using System.Reflection;
using Conduit.Core.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Conduit.Core.PipelineBehaviors.Transactions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddTransactionPipelineBehaviorsFromAssembly(this IServiceCollection services, Assembly domainContractsAssembly, Type moduleType)
        {
            domainContractsAssembly.GetOperationContractTypes().ForEach(operationType =>
            {
                /*
                 * need to enumerate operation contract types and register TransactionPipelineBehavior<TRequest, TResponse, TModule> for all of them equivalent to:
                 *
                 * services
                 *   .AddTransient<
                 *      IPipelineBehavior<RegisterUserCommand, OperationResponse<RegisterUserCommandResult>>, 
                 *      TransactionPipelineBehavior<RegisterUserCommand, OperationResponse<RegisterUserCommandResult>, UsersModule>>()
                 *
                 * have to do this because the arity differs between service type and implementation type :(
                 * this is due to needing the TModule type to ensure unique DI registrations for db connections per module
                 *
                 * The standard trick we usually apply (like below) doesn't work when the arity doesn't match
                 * services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionPipelineBehavior<,,>)); //this blows up
                 */
                
                var rawPipelineBehaviorType = typeof(IPipelineBehavior<,>);
                var rawTransactionPipelineBehaviorType = typeof(TransactionPipelineBehavior<,,>);
            
                var requestType = operationType;
                var responseType = requestType.GetInterfaces().Single(i => i.Name.Contains("IRequest")).GenericTypeArguments.Single();

                var pipelineBehaviorType = rawPipelineBehaviorType.MakeGenericType(requestType, responseType);
                var transactionBehaviorType = rawTransactionPipelineBehaviorType.MakeGenericType(requestType, responseType, moduleType);
            
                var descriptor = new ServiceDescriptor(pipelineBehaviorType, transactionBehaviorType, ServiceLifetime.Transient);
                services.Add(descriptor);
            });
        }
    }
}