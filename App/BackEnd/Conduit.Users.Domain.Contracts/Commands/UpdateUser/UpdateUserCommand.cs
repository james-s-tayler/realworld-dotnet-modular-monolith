using Conduit.Core.DataAccess;
using Conduit.Core.PipelineBehaviors;
using MediatR;

namespace Conduit.Users.Domain.Contracts.Commands.UpdateUser
{
    public class UpdateUserCommand : ContractModel, IRequest<OperationResponse<UpdateUserCommandResult>>
    {
        public UpdateUserDTO UpdateUser { get; set; }
    }
}