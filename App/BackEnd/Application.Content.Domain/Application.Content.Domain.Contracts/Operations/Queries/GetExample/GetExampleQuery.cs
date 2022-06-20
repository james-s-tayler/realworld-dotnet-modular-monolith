using Application.Core.DataAccess;
using Application.Core.PipelineBehaviors.OperationResponse;
using MediatR;

namespace Application.Content.Domain.Contracts.Operations.Queries.GetExample
{
    public class GetExampleQuery : ContractModel, IRequest<OperationResponse<GetExampleQueryResult>>
    {
        public int Id { get; set; }
    }
}