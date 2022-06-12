using Conduit.Core.DataAccess;
using Conduit.Core.PipelineBehaviors.OperationResponse;
using MediatR;

namespace Conduit.Users.Domain.Contracts.Queries.GetProfile
{
    public class GetProfileQuery : ContractModel, IRequest<OperationResponse<GetProfileQueryResult>>
    {
        public string Username { get; set; }
    }
}