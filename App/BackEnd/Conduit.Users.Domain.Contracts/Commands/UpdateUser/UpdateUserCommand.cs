using Application.Core.DataAccess;
using Application.Core.PipelineBehaviors.OperationResponse;
using MediatR;

namespace Conduit.Users.Domain.Contracts.Commands.UpdateUser
{
    public class UpdateUserCommand : ContractModel, IRequest<OperationResponse<UpdateUserCommandResult>>
    {
        public UpdateUserDTO UpdateUser { get; set; }
    }
}