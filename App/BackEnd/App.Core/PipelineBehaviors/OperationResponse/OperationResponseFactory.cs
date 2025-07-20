using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using MediatR;

namespace App.Core.PipelineBehaviors.OperationResponse
{
    public static class OperationResponseFactory
    {
        public static OperationResponse<TResult> Success<TResult>(TResult result) where TResult : class
        {
            return new OperationResponse<TResult>(result);
        }

        public static TResponse NotFound<TRequest, TResponse>(Type entityType, object identifier) where TResponse : class where TRequest : IRequest<TResponse>
        {
            var errorMessages = new List<string> { $"{entityType.Name.Replace("Entity", "")} '{identifier}' was not found." };
            return CreateResponse<TRequest, TResponse>(OperationResult.NotFound, errorMessages);
        }

        public static TResponse InvalidRequest<TRequest, TResponse>([NotNull] List<string> errorMessages) where TResponse : class where TRequest : IRequest<TResponse>
        {
            return CreateResponse<TRequest, TResponse>(OperationResult.InvalidRequest, errorMessages);
        }

        public static TResponse ValidationError<TRequest, TResponse>([NotNull] List<string> errorMessages) where TResponse : class where TRequest : IRequest<TResponse>
        {
            return CreateResponse<TRequest, TResponse>(OperationResult.ValidationError, errorMessages);
        }

        public static TResponse NotAuthenticated<TRequest, TResponse>() where TResponse : class where TRequest : IRequest<TResponse>
        {
            var errorMessages = new List<string> { "Not authenticated" };
            return CreateResponse<TRequest, TResponse>(OperationResult.NotAuthenticated, errorMessages);
        }

        public static TResponse NotAuthorized<TRequest, TResponse>([NotNull] string failureMessage) where TResponse : class where TRequest : IRequest<TResponse>
        {
            var errorMessages = new List<string>
            {
                "Not authorized",
                failureMessage
            };
            return CreateResponse<TRequest, TResponse>(OperationResult.NotAuthorized, errorMessages);
        }

        public static TResponse NotImplemented<TRequest, TResponse>() where TResponse : class where TRequest : IRequest<TResponse>
        {
            var errorMessages = new List<string> { "Not implemented" };
            return CreateResponse<TRequest, TResponse>(OperationResult.NotImplemented, errorMessages);
        }

        public static TResponse UnhandledException<TRequest, TResponse>([NotNull] Exception e) where TResponse : class where TRequest : IRequest<TResponse>
        {
            var resultType = typeof(TResponse).GetGenericArguments()[0];
            var operationResponseType = typeof(OperationResponse<>).MakeGenericType(resultType);
            return Activator.CreateInstance(operationResponseType, e) as TResponse;
        }

        private static TResponse CreateResponse<TRequest, TResponse>(OperationResult result, List<string> errorMessages) where TResponse : class where TRequest : IRequest<TResponse>
        {
            var resultType = typeof(TResponse).GetGenericArguments()[0];
            var operationResponseType = typeof(OperationResponse<>).MakeGenericType(resultType);
            return Activator.CreateInstance(operationResponseType, errorMessages, result) as TResponse;
        }
    }
}