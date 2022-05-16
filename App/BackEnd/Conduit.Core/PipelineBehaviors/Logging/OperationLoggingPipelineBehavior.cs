using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Conduit.Core.PipelineBehaviors.Logging 
{
    
    public class OperationLoggingPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
        where TResponse : class
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<OperationLoggingPipelineBehavior<TRequest, TResponse>> _logger;

        public OperationLoggingPipelineBehavior(ILogger<OperationLoggingPipelineBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (!IsOperationResponse())
                throw new InvalidOperationException("Domain operations must be of type OperationResponse<T>");

            var requestTypeName = typeof(TRequest).Name;
            _logger.LogInformation("Operation {@Request}", request);
            TResponse response;
            try
            {
                response = await next();
            }
            catch (Exception e)
            {
                response = OperationResponseFactory.UnhandledException<TRequest, TResponse>(e);
            }

            var responseSummary = response as IOperationResponseSummary;

            if (responseSummary!.IsSuccess())
            {
                _logger.LogInformation("Operation {Operation} {Result}", requestTypeName, responseSummary.Result);    
            }
            else
            {
                _logger.LogError("Operation {Operation} {Result}: {@Errors}", requestTypeName, responseSummary.Result, responseSummary.Errors);    
            }


            return response;
        }

        private bool IsOperationResponse() //make this an extension method maybe?
        {
            var responseType = typeof(TResponse);
            return responseType.IsGenericType && responseType.Name.Contains("OperationResponse");
        }
    }
}