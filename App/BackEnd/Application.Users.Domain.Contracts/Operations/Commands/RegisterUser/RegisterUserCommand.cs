using Application.Core.DataAccess;
using Application.Core.PipelineBehaviors.Authorization;
using Application.Core.PipelineBehaviors.OperationResponse;
using Application.Users.Domain.Contracts.DTOs;
using MediatR;

namespace Application.Users.Domain.Contracts.Operations.Commands.RegisterUser
{
    [AllowUnauthenticated]
    public class RegisterUserCommand : ContractModel, IRequest<OperationResponse<RegisterUserCommandResult>>
    {
        public NewUserDTO NewUser { get; set; }
    } 
}