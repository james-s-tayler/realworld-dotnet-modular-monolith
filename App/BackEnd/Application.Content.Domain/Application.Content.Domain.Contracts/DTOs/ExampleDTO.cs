using Application.Core.DataAccess;
using Destructurama.Attributed;

namespace Application.Content.Domain.Contracts.DTOs
{
    public class ExampleDTO : ContractModel
    {
        public int Id { get; set; }
        [NotLogged] 
        public string SensitiveValue { get; set; }
    }
}