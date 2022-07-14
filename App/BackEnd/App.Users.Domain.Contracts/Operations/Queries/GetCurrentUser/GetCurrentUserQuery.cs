using App.Core.DataAccess;
using App.Core.PipelineBehaviors.OperationResponse;
using MediatR;

namespace App.Users.Domain.Contracts.Operations.Queries.GetCurrentUser
{
    public class GetCurrentUserQuery : ContractModel, IRequest<OperationResponse<GetCurrentUserQueryResult>> {}
}