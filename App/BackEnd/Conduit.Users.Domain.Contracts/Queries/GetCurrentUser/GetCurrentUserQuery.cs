using Conduit.Core.PipelineBehaviors;
using MediatR;

namespace Conduit.Identity.Domain.Contracts.Queries.GetCurrentUser
{
    public class GetCurrentUserQuery : IRequest<OperationResponse<GetCurrentUserQueryResult>> {}
}