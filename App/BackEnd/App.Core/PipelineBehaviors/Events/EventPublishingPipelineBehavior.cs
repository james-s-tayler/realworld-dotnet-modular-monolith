using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using App.Core.PipelineBehaviors.OperationResponse;
using MediatR;

namespace App.Core.PipelineBehaviors.Events
{
    public class EventPublishingPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : class
    {
        private readonly IMediator _mediator;

        public EventPublishingPipelineBehavior(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (!typeof(TResponse).IsOperationResponse())
                throw new InvalidOperationException("Domain operations must be of type OperationResponse<T>");

            var operationResponse = await next();
            var operationResponseSummary = (IOperationResponseSummary)operationResponse;
            var responseType = typeof(TResponse).GenericTypeArguments.Single();

            if (ShouldPublishEvent(responseType, operationResponseSummary))
            {
                //need to chaos test what happens when Publish blows up
                await _mediator.Publish(operationResponseSummary.GetResponse(), cancellationToken);
            }
            
            return operationResponse;
        }

        private static bool ShouldPublishEvent(Type resultType, IOperationResponseSummary operationResponse)
        {
            return resultType.GetInterfaces().Any(interfaceType => interfaceType == typeof(INotification)) && 
                   operationResponse.IsSuccess() &&
                   operationResponse.GetResponse() != null;
        }
    }
}