using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Conduit.Core.Validation
{
    public class ValidateableResponse<T> where T : class
    {
        private readonly IList<string> _errorMessages = new List<string>();
        public HttpStatusCode HttpStatusCode { get; } = HttpStatusCode.OK;
        public T Result { get; }

        public ValidateableResponse(T model)
        {
            Result = model;
        }
        
        public ValidateableResponse(List<string> errorMessages, HttpStatusCode httpStatusCode)
        {
            _errorMessages = errorMessages;
            HttpStatusCode = httpStatusCode;
        }
        
        public bool IsValidResponse => !_errorMessages.Any();
        public string ErrorMessage => string.Join(",", _errorMessages);
    }
}