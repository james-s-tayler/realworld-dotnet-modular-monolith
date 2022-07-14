using App.Core.DataAccess;
using App.Core.PipelineBehaviors.Authorization;
using App.Core.PipelineBehaviors.OperationResponse;
using MediatR;

namespace App.Content.Domain.Contracts.Operations.Queries.GetTags
{
    [AllowUnauthenticated]
    public class GetTagsQuery : ContractModel, IRequest<OperationResponse<GetTagsQueryResult>>
    {
    }
}