namespace App.Core.PipelineBehaviors.OperationResponse
{
    public enum OperationResult
    {
        Success,
        InvalidRequest,
        ValidationError,
        NotFound,
        NotAuthenticated,
        NotAuthorized,
        NotImplemented,
        UnhandledException
    }
}