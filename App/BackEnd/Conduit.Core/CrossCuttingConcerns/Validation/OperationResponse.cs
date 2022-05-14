using System.Collections.Generic;

namespace Conduit.Core.Validation.CrossCuttingConcerns.Validation
{
    public class OperationResponse<T> where T : class
    {
        public OperationResult Result { get; } = OperationResult.Success;
        
        public List<string> ErrorMessages { get; } = new ();
        
        public T Response { get; }

        public OperationResponse(T model)
        {
            Response = model;
        }
        
        public OperationResponse(List<string> errorMessages, OperationResult result)
        {
            ErrorMessages = errorMessages;
            Result = result;
        }
        
        public string ErrorMessage => string.Join(",", ErrorMessages);
    }
}