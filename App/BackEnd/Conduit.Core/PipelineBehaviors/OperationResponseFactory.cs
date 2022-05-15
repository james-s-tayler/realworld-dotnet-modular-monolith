using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;
using MediatR;

namespace Conduit.Core.PipelineBehaviors
{
    public class OperationResponseFactory
    {
        public static TResponse ValidationError<TRequest, TResponse>(ValidationResult validationResult) where TResponse : class where TRequest : IRequest<TResponse>
        {
            var requestType = typeof(TRequest);
            var errorMessages = new List<string> { $"Validation error(s) occurred while performing {requestType.Name}"};
            errorMessages.AddRange(validationResult.Errors.Select(s => s.ErrorMessage).ToList());
            return CreateResponse<TRequest, TResponse>(OperationResult.ValidationError, errorMessages);
        }
        
        public static TResponse NotAuthenticated<TRequest, TResponse>() where TResponse : class where TRequest : IRequest<TResponse>
        {
            var requestType = typeof(TRequest);
            var errorMessages = new List<string>{$"Must be authenticated to perform {requestType.Name}"};
            return CreateResponse<TRequest, TResponse>(OperationResult.NotAuthenticated, errorMessages);
        }
        
        public static TResponse NotAuthorized<TRequest, TResponse>(string failureMessage) where TResponse : class where TRequest : IRequest<TResponse>
        {
            var requestType = typeof(TRequest);
            var errorMessages = new List<string>
            {
                $"Not authorized to perform {requestType.Name}",
                failureMessage
            };
            return CreateResponse<TRequest, TResponse>(OperationResult.NotAuthorized, errorMessages);
        }
        
        public static TResponse NotImplemented<TRequest, TResponse>() where TResponse : class where TRequest : IRequest<TResponse>
        {
            var requestType = typeof(TRequest);
            var errorMessages = new List<string>{$"{requestType.Name} has not been implemented"};
            return CreateResponse<TRequest, TResponse>(OperationResult.NotImplemented, errorMessages);
        }
        
        private static TResponse CreateResponse<TRequest, TResponse>(OperationResult result, List<string> errorMessages) where TResponse : class where TRequest : IRequest<TResponse>
        {
            var resultType = typeof(TResponse).GetGenericArguments()[0];
            var operationResponseType = typeof(OperationResponse<>).MakeGenericType(resultType);
            return Activator.CreateInstance(operationResponseType, errorMessages, result) as TResponse;
        }
    }
}