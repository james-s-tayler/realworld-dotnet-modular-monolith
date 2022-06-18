using Application.Core.DataAccess;
using Application.Core.PipelineBehaviors.OperationResponse;
using Application.Users.Domain.Contracts.DTOs;
using MediatR;

namespace Application.Users.Domain.Contracts.Operations.Commands.UpdateUser
{
    public class UpdateUserCommand : ContractModel, IRequest<OperationResponse<UpdateUserCommandResult>>
    {
        public UpdateUserDTO UpdateUser { get; set; }
    }
}