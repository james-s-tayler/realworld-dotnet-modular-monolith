using System;
using System.Collections.Generic;
using Conduit.Core.PipelineBehaviors.Logging;

namespace Conduit.Core.PipelineBehaviors
{
    public class OperationResponse<T> : IOperationResponseSummary where T : class
    {
        public OperationResult Result { get; } = OperationResult.Success;
        
        public List<string> Errors { get; } = new ();
        
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
}