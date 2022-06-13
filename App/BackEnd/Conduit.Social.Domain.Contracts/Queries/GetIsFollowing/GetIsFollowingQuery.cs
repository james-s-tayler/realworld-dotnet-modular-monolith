using Conduit.Core.DataAccess;
using Conduit.Core.PipelineBehaviors.OperationResponse;
using MediatR;

namespace Conduit.Social.Domain.Contracts.Queries.GetIsFollowing
{
    public class GetIsFollowingQuery : ContractModel, IRequest<OperationResponse<GetIsFollowingQueryResult>>
    {
        public string Username { get; set; }
    }
}