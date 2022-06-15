using Application.Core.DataAccess;
using Application.Core.PipelineBehaviors.OperationResponse;
using MediatR;

namespace Conduit.Social.Domain.Contracts.Commands.UnfollowUser
{
    public class UnfollowUserCommand : ContractModel, IRequest<OperationResponse<UnfollowUserCommandResult>>
    {
        public string Username { get; set; }
    }
}