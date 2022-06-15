using Application.Core.DataAccess;
using Application.Core.PipelineBehaviors.OperationResponse;
using MediatR;

namespace Application.Users.Domain.Contracts.Commands.UpdateUser
{
    public class UpdateUserCommand : ContractModel, IRequest<OperationResponse<UpdateUserCommandResult>>
    {
        public UpdateUserDTO UpdateUser { get; set; }
    }
}