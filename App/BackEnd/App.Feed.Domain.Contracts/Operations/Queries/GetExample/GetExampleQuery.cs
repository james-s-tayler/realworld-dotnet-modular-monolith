using App.Core.DataAccess;
using App.Core.PipelineBehaviors.OperationResponse;
using MediatR;

namespace App.Feed.Domain.Contracts.Operations.Queries.GetExample
{
    public class GetExampleQuery : ContractModel, IRequest<OperationResponse<GetExampleQueryResult>>
    {
        public int Id { get; set; }
    }
}