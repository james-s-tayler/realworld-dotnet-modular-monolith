using Application.Core.DataAccess;
using Application.Core.PipelineBehaviors.OperationResponse;
using MediatR;

namespace Application.Social.Domain.Contracts.Operations.Commands.FollowUser
{
    public class FollowUserCommand : ContractModel, IRequest<OperationResponse<FollowUserCommandResult>>
    {
        public string Username { get; set; }
    }
}