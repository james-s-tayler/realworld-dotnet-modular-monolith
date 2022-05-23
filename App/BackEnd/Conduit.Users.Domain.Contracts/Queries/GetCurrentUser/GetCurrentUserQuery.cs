using Conduit.Core.DataAccess;
using Conduit.Core.PipelineBehaviors;
using MediatR;

namespace Conduit.Users.Domain.Contracts.Queries.GetCurrentUser
{
    public class GetCurrentUserQuery : ContractModel, IRequest<OperationResponse<GetCurrentUserQueryResult>> {}
}