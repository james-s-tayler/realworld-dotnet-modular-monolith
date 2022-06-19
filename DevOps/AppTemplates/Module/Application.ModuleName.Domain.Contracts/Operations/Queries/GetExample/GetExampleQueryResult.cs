using Application.Core.DataAccess;
using Application.ModuleName.Domain.Contracts.DTOs;

namespace Application.ModuleName.Domain.Contracts.Operations.Queries.GetExample
{
    public class GetExampleQueryResult : ContractModel
    {
        public ExampleDTO ExampleOutput { get; set; }
    }
}