namespace Application.Core.PipelineBehaviors.Validation
{
    public interface IInputContractValidator
    {
        bool IsContractAdheredTo<T>(T input);
    }
}