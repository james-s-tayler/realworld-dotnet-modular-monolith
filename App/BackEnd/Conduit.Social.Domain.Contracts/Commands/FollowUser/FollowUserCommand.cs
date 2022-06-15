using Application.Core.DataAccess;
using Application.Core.PipelineBehaviors.OperationResponse;
using MediatR;

namespace Conduit.Social.Domain.Contracts.Commands.FollowUser
{
    public class FollowUserCommand : ContractModel, IRequest<OperationResponse<FollowUserCommandResult>>
    {
        public string Username { get; set; }
    }
}