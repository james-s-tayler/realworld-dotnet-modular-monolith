using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.PipelineBehaviors.OperationResponse;
using FluentValidation;
using FluentValidation.Results;
using JetBrains.Annotations;
using MediatR;

namespace Application.Core.PipelineBehaviors.Validation
{
    //adapted from: https://medium.com/the-cloud-builders-guild/validation-without-exceptions-using-a-mediatr-pipeline-behavior-278f124836dc
    public class ValidationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
        where TResponse : class
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _commandValidators;
        private readonly IInputContractValidator _inputContractValidator;

        public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> commandValidators, [NotNull] IInputContractValidator inputContractValidator)
        {
            _commandValidators = commandValidators;
            _inputContractValidator = inputContractValidator;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (!typeof(TResponse).IsOperationResponse())
                throw new InvalidOperationException("Domain operations must be of type OperationResponse<T>");

            if (!_inputContractValidator.IsContractAdheredTo(request))
            {
                return OperationResponseFactory.ValidationError<TRequest, TResponse>(new List<string> { "The input contract was not adhered to" });
            }
                
            if(!HasValidators())
                return await next();

            var validationResult = await DoValidation(request, cancellationToken);

            if (validationResult.IsValid)
            {
                return await next();
            }

            return OperationResponseFactory.ValidationError<TRequest, TResponse>(validationResult.Errors.Select(s => s.ErrorMessage).ToList());
        }

        private async Task<ValidationResult> DoValidation(TRequest request, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRequest>(request);
            var validationResults = await Task.WhenAll(_commandValidators.Select(v => v.ValidateAsync(context, cancellationToken)));
            var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();
            return new ValidationResult(failures);
        }

        private bool HasValidators()
        {
            if (_commandValidators == null || !_commandValidators.Any())
                return false; //obviously we can't validate anything with no validators
            
            return true;
        }
    }
}