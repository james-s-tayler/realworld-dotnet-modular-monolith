using App.Core.DataAccess;
using Destructurama.Attributed;

namespace App.Feed.Domain.Contracts.DTOs
{
    public class ExampleDTO : ContractModel
    {
        public int Id { get; set; }
        [NotLogged]
        public string SensitiveValue { get; set; }
    }
}