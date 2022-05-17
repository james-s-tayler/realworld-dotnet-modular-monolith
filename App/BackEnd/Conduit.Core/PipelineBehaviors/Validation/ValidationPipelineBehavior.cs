using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Conduit.Core.PipelineBehaviors.Validation
{
    //adapted from: https://medium.com/the-cloud-builders-guild/validation-without-exceptions-using-a-mediatr-pipeline-behavior-278f124836dc
    public class ValidationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
        where TResponse : class
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _commandValidators;
        private readonly ILogger<ValidationPipelineBehavior<TRequest, TResponse>> _logger;

        public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> commandValidators,
            ILogger<ValidationPipelineBehavior<TRequest, TResponse>> logger)
        {
            _commandValidators = commandValidators;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest command, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (!typeof(TResponse).IsOperationResponse())
                throw new InvalidOperationException("Domain operations must be of type OperationResponse<T>");
            
            if(!IsValidatable())
                return await next();

            var validationResult = await DoValidation(command, cancellationToken);

            if (validationResult.IsValid)
            {
                return await next();
            }

            return OperationResponseFactory.ValidationError<TRequest, TResponse>(validationResult);
        }

        private async Task<ValidationResult> DoValidation(TRequest command, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRequest>(command);
            var validationResults = await Task.WhenAll(_commandValidators.Select(v => v.ValidateAsync(context, cancellationToken)));
            var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();
            return new ValidationResult(failures);
        }

        private bool IsValidatable()
        {
            if (_commandValidators == null || !_commandValidators.Any())
                return false; //obviously we can't validate anything with no validators
            
            return true;
        }
    }
}