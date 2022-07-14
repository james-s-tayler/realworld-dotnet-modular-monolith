using App.Core.DataAccess;
using App.Feed.Domain.Contracts.DTOs;

namespace App.Feed.Domain.Contracts.Operations.Queries.GetExample
{
    public class GetExampleQueryResult : ContractModel
    {
        public ExampleDTO ExampleOutput { get; set; }
    }
}