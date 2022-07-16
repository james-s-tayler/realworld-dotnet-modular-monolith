using App.Core.DataAccess;
using App.ModuleName.Domain.Contracts.DTOs;

namespace App.ModuleName.Domain.Contracts.Operations.Queries.GetExample
{
    public class GetExampleQueryResult : ContractModel
    {
        public ExampleDTO ExampleOutput { get; set; }
    }
}