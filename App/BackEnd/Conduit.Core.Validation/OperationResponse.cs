using System.Collections.Generic;
using System.Linq;

namespace Conduit.Core.Validation
{
    public class OperationResponse<T> where T : class
    {
        public OperationResult Result { get; } = OperationResult.Success;
        
        private readonly IList<string> _errorMessages = new List<string>();
        
        public T Response { get; }

        public OperationResponse(T model)
        {
            Response = model;
        }
        
        public OperationResponse(List<string> errorMessages, OperationResult result)
        {
            _errorMessages = errorMessages;
            Result = result;
        }
        
        public string ErrorMessage => string.Join(",", _errorMessages);
    }
}