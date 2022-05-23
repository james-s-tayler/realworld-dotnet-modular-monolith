using Conduit.Core.DataAccess;
using Conduit.Core.PipelineBehaviors;
using MediatR;

namespace Conduit.Users.Domain.Contracts.Commands.UpdateUser
{
    public class UpdateUserCommand : ContractModel, IRequest<OperationResponse<UpdateUserResult>>
    {
        public UpdateUserDTO UpdateUser { get; set; }
    }
}