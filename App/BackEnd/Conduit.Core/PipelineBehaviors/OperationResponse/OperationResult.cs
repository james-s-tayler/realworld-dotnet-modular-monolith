namespace Conduit.Core.PipelineBehaviors.OperationResponse
{
    public enum OperationResult
    {
        Success,
        ValidationError,
        NotAuthenticated,
        NotAuthorized,
        NotImplemented,
        UnhandledException
    }
}