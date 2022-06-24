using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Application.Core.PipelineBehaviors.Validation
{
    //adapted from: https://stackoverflow.com/questions/7663501/dataannotations-recursively-validating-an-entire-object-graph
    public interface IDataAnnotationsValidator
    {
        bool TryValidateObject(object obj, ICollection<ValidationResult> results, IDictionary<object, object> validationContextItems = null);
        bool TryValidateObjectRecursive<T>(T obj, List<ValidationResult> results, IDictionary<object, object> validationContextItems = null);
    }
}