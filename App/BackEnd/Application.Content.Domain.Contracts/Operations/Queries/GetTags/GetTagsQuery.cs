using Application.Core.DataAccess;
using Application.Core.PipelineBehaviors.Authorization;
using Application.Core.PipelineBehaviors.OperationResponse;
using MediatR;

namespace Application.Content.Domain.Contracts.Operations.Queries.GetTags
{
    [AllowUnauthenticated]
    public class GetTagsQuery : ContractModel, IRequest<OperationResponse<GetTagsQueryResult>>
    {
    }
}