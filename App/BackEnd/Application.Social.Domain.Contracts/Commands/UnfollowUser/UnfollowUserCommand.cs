using Application.Core.DataAccess;
using Application.Core.PipelineBehaviors.OperationResponse;
using MediatR;

namespace Application.Social.Domain.Contracts.Commands.UnfollowUser
{
    public class UnfollowUserCommand : ContractModel, IRequest<OperationResponse<UnfollowUserCommandResult>>
    {
        public string Username { get; set; }
    }
}