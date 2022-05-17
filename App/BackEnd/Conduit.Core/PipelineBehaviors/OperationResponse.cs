using System;
using System.Collections.Generic;
using Conduit.Core.Exceptions;
using Conduit.Core.PipelineBehaviors.Logging;

namespace Conduit.Core.PipelineBehaviors
{
    public class OperationResponse<T> : IOperationResponseSummary where T : class
    {
        public OperationResult Result { get; } = OperationResult.Success;
        public List<string> Errors { get; } = new ();
        public Exception Exception { get; } = null;
        public T Response { get; }

        public OperationResponse(T model)
        {
            Response = model;
        }
        
        public OperationResponse(List<string> errors, OperationResult result)
        {
            Errors = errors;
            Result = result;
        }
        
        public OperationResponse(Exception e)
        {
            Exception = e ?? throw new ArgumentNullException(nameof(e));
            Errors = e.GetErrorMessages();
            Result = OperationResult.UnhandledException;
        }
        
        public string ErrorMessage => string.Join(";", Errors);

        public bool IsSuccess()
        {
            return Result == OperationResult.Success;
        }

        public Type GetResponseType()
        {
            return typeof(T);
        }
    }

    public static class TypeExtensions
    {
        public static bool IsOperationResponse(this Type type)
        {
            if (!type.IsGenericType)
                return false;
            
            return type.Name.StartsWith($"OperationResponse`{type.GenericTypeArguments.Length}");
        }
    }
}