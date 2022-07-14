using App.Core.DataAccess;
using App.Core.PipelineBehaviors.OperationResponse;
using MediatR;

namespace App.Social.Domain.Contracts.Operations.Commands.UnfollowUser
{
    public class UnfollowUserCommand : ContractModel, IRequest<OperationResponse<UnfollowUserCommandResult>>
    {
        public string Username { get; set; }
    }
}