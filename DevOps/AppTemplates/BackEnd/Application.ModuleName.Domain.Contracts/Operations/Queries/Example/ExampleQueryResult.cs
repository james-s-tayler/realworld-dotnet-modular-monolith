using Application.Core.DataAccess;
using Application.ModuleName.Domain.Contracts.DTOs;

namespace Application.ModuleName.Domain.Contracts.Operations.Queries.Example
{
    public class ExampleQueryResult : ContractModel
    {
        public ExampleDTO ExampleOutput { get; set; }
    }
}