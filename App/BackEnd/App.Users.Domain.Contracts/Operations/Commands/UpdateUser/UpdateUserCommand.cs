using App.Core.DataAccess;
using App.Core.PipelineBehaviors.OperationResponse;
using App.Users.Domain.Contracts.DTOs;
using MediatR;

namespace App.Users.Domain.Contracts.Operations.Commands.UpdateUser
{
    public class UpdateUserCommand : ContractModel, IRequest<OperationResponse<UpdateUserCommandResult>>
    {
        public UpdateUserDTO UpdateUser { get; set; }
    }
}