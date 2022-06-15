using Application.Core.DataAccess;
using Application.Core.PipelineBehaviors.OperationResponse;
using MediatR;

namespace Application.Users.Domain.Contracts.Queries.GetCurrentUser
{
    public class GetCurrentUserQuery : ContractModel, IRequest<OperationResponse<GetCurrentUserQueryResult>> {}
}