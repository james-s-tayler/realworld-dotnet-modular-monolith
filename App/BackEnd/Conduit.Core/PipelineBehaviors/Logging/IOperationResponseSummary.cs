using System;
using System.Collections.Generic;

namespace Conduit.Core.PipelineBehaviors.Logging
{
    public interface IOperationResponseSummary
    {
        OperationResult Result { get; }
        List<string> Errors { get; }
        Exception Exception { get; }
        bool IsSuccess();
        Type GetResponseType();
    }
}