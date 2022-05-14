using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace Conduit.Core.PipelineBehaviors
{
    public class OperationResponseFactory
    {
        public static TResponse ValidationError<TResponse>(ValidationResult validationResult) where TResponse : class
        {
            var resultType = typeof(TResponse).GetGenericArguments()[0];
            var operationResponseType = typeof(OperationResponse<>).MakeGenericType(resultType);

            var errorMessages = validationResult.Errors.Select(s => s.ErrorMessage).ToList();
            return Activator.CreateInstance(operationResponseType, errorMessages, OperationResult.ValidationError) as TResponse;
        }
        
        public static TResponse NotAuthenticated<TResponse>() where TResponse : class
        {
            var resultType = typeof(TResponse).GetGenericArguments()[0];
            var operationResponseType = typeof(OperationResponse<>).MakeGenericType(resultType);

            var errorMessages = new List<string>{"No user has been authenticated."};
            return Activator.CreateInstance(operationResponseType, errorMessages, OperationResult.NotAuthenticated) as TResponse;
        }
        
        public static TResponse NotAuthorized<TResponse>(string failureMessage) where TResponse : class
        {
            var resultType = typeof(TResponse).GetGenericArguments()[0];
            var operationResponseType = typeof(OperationResponse<>).MakeGenericType(resultType);

            var errorMessages = new List<string>{failureMessage};
            return Activator.CreateInstance(operationResponseType, errorMessages, OperationResult.NotAuthorized) as TResponse;
        }
    }
}