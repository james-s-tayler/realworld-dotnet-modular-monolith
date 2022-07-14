using App.Core.DataAccess;
using App.Core.PipelineBehaviors.Authorization;
using App.Core.PipelineBehaviors.OperationResponse;
using App.Users.Domain.Contracts.DTOs;
using MediatR;

namespace App.Users.Domain.Contracts.Operations.Commands.RegisterUser
{
    [AllowUnauthenticated]
    public class RegisterUserCommand : ContractModel, IRequest<OperationResponse<RegisterUserCommandResult>>
    {
        public NewUserDTO NewUser { get; set; }
    } 
}