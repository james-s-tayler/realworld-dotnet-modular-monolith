using System;
using System.Collections.Generic;

namespace Application.Core.PipelineBehaviors.OperationResponse
{
    public interface IOperationResponseSummary
    {
        OperationResult Result { get; }
        List<string> Errors { get; }
        Exception Exception { get; }
        bool IsSuccess();
        Type GetResponseType();
        object GetResponse();
    }
}