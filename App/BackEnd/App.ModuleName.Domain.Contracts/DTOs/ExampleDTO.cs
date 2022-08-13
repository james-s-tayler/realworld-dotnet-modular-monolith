using System.Diagnostics.CodeAnalysis;
using App.Core.DataAccess;
using Destructurama.Attributed;

namespace App.ModuleName.Domain.Contracts.DTOs
{
    [ExcludeFromCodeCoverage]
    public class ExampleDTO : ContractModel
    {
        public int Id { get; set; }
        [NotLogged]
        public string SensitiveValue { get; set; }
    }
}