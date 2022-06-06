using System;
using System.Threading;
using System.Threading.Tasks;
using Conduit.Core.Context;
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
            if (!typeof(TResponse).IsOperationResponse())
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

            if (responseSummary!.Result == OperationResult.UnhandledException)
            {
                //for better signal to noise ratio in logs treat only things that our application was not expecting to happen as errors
                //validation errors, failed auth etc are things our application knows about and expects to happen hence are information level logs
                _logger.LogError(responseSummary!.Exception, "Operation {Operation} {Result}: {@Errors}", requestTypeName, responseSummary.Result, responseSummary.Errors);
            }
            else if(responseSummary!.Result == OperationResult.Success)
            {
                _logger.LogInformation("Operation {Operation} {Result}", requestTypeName, responseSummary.Result);
            }
            else
            {
                _logger.LogInformation("Operation {Operation} {Result}: {@Errors}", requestTypeName, responseSummary.Result, responseSummary.Errors);
            }
            
            return response;
        }
    }
}