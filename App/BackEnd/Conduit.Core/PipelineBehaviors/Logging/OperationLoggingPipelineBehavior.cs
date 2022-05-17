using System;
using System.Threading;
using System.Threading.Tasks;
using Conduit.Core.Context;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace Conduit.Core.PipelineBehaviors.Logging 
{
    
    public class OperationLoggingPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
        where TResponse : class
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<OperationLoggingPipelineBehavior<TRequest, TResponse>> _logger;
        private readonly IUserContext _userContext;

        public OperationLoggingPipelineBehavior(ILogger<OperationLoggingPipelineBehavior<TRequest, TResponse>> logger, 
            IUserContext userContext)
        {
            _logger = logger;
            _userContext = userContext;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (!IsOperationResponse())
                throw new InvalidOperationException("Domain operations must be of type OperationResponse<T>");

            var requestTypeName = typeof(TRequest).Name;
            using (LogContext.PushProperty("Username", _userContext.IsAuthenticated ? _userContext.Username : "Anonymous"))
            {
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

        private bool IsOperationResponse() //make this an extension method maybe?
        {
            var responseType = typeof(TResponse);
            return responseType.IsGenericType && responseType.Name.Contains("OperationResponse");
        }
    }
}