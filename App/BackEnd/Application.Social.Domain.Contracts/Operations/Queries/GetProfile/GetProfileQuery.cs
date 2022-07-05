using Application.Core.DataAccess;
using Application.Core.PipelineBehaviors.Authorization;
using Application.Core.PipelineBehaviors.OperationResponse;
using MediatR;

namespace Application.Social.Domain.Contracts.Operations.Queries.GetProfile
{
    [AllowUnauthenticated]
    public class GetProfileQuery : ContractModel, IRequest<OperationResponse<GetProfileQueryResult>>
    {
        public string Username { get; set; }
    }
}