using App.Core.DataAccess;
using App.Core.PipelineBehaviors.Authorization;
using App.Core.PipelineBehaviors.OperationResponse;
using MediatR;

namespace App.Social.Domain.Contracts.Operations.Queries.GetProfile
{
    [AllowUnauthenticated]
    public class GetProfileQuery : ContractModel, IRequest<OperationResponse<GetProfileQueryResult>>
    {
        public string Username { get; set; }
    }
}