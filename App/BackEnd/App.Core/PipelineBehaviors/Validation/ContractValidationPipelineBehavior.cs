using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using App.Core.PipelineBehaviors.OperationResponse;
using JetBrains.Annotations;
using MediatR;

namespace App.Core.PipelineBehaviors.Validation
{
    public class ContractValidationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TResponse : class
        where TRequest : IRequest<TResponse>
    {
        private readonly IInputContractValidator _inputContractValidator;

        public ContractValidationPipelineBehavior([NotNull] IInputContractValidator inputContractValidator)
        {
            _inputContractValidator = inputContractValidator;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if ( !typeof(TResponse).IsOperationResponse() )
                throw new InvalidOperationException("Domain operations must be of type OperationResponse<T>");

            if ( !_inputContractValidator.IsContractAdheredTo(request) )
            {
                return OperationResponseFactory.InvalidRequest<TRequest, TResponse>(new List<string> { "The input contract was not adhered to" });
            }

            return await next();
        }
    }
}