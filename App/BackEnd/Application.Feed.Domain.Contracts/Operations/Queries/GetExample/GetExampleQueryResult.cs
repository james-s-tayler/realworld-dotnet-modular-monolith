using Application.Core.DataAccess;
using Application.Feed.Domain.Contracts.DTOs;

namespace Application.Feed.Domain.Contracts.Operations.Queries.GetExample
{
    public class GetExampleQueryResult : ContractModel
    {
        public ExampleDTO ExampleOutput { get; set; }
    }
}