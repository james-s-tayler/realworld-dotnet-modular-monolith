using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Conduit.Core.Validation
{
    //adapted from: https://medium.com/the-cloud-builders-guild/validation-without-exceptions-using-a-mediatr-pipeline-behavior-278f124836dc
    public class CommandValidationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
        where TResponse : class
        where TRequest : IRequest<TResponse>, IValidateable //apply to all Commands marked with IValidateable marker interface
    {
        private readonly IEnumerable<IValidator<TRequest>> _commandValidators;
        private readonly ILogger<CommandValidationPipelineBehavior<TRequest, TResponse>> _logger;

        public CommandValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> commandValidators,
            ILogger<CommandValidationPipelineBehavior<TRequest, TResponse>> logger)
        {
            _commandValidators = commandValidators;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest command, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if(!IsValidatable()) 
                return await next();

            var validationResult = await DoValidation(command, cancellationToken);

            if (validationResult.IsValid)
            {
                return await next();
            }

            _logger.LogError("Validation Error: " + validationResult.Errors.Select(s => s.ErrorMessage)
                        .Aggregate((acc, curr) => acc += string.Concat("_|_", curr)));

            return CreateValidatableResponseWithErrors(validationResult);
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
            return _commandValidators.Any() ||       //obviously we can't validate anything with no validators
                   !typeof(TResponse).IsGenericType; //and we can only handle ValidatableResponse<T>
        }

        private TResponse CreateValidatableResponseWithErrors(ValidationResult validationResult)
        {
            var resultType = typeof(TResponse).GetGenericArguments()[0];
            var invalidResponseType = typeof(ValidateableResponse<>).MakeGenericType(resultType);

            var errorMessages = validationResult.Errors.Select(s => s.ErrorMessage).ToList();
            var httpStatusCode = HttpStatusCode.BadRequest;
            return Activator.CreateInstance(invalidResponseType, errorMessages, httpStatusCode) as TResponse;
        }
    }
}