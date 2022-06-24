using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Application.Core.PipelineBehaviors.Validation
{
    public class InputContractValidator : IInputContractValidator
    {
        private readonly IDataAnnotationsValidator _validator;

        public InputContractValidator(IDataAnnotationsValidator validator)
        {
            _validator = validator;
        }

        public bool IsContractAdheredTo<T>(T input)
        {
            return _validator.TryValidateObjectRecursive(input, new List<ValidationResult>());
        }
    }
}