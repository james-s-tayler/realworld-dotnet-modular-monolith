using Application.Core.DataAccess;
using Application.Content.Domain.Contracts.DTOs;

namespace Application.Content.Domain.Contracts.Operations.Queries.GetExample
{
    public class GetExampleQueryResult : ContractModel
    {
        public ExampleDTO ExampleOutput { get; set; }
    }
}