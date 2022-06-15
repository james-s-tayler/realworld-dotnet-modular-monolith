using Application.Core.DataAccess;
using Application.Core.PipelineBehaviors.Authorization;
using Application.Core.PipelineBehaviors.OperationResponse;
using MediatR;

namespace Conduit.Users.Domain.Contracts.Commands.RegisterUser
{
    [AllowUnauthenticated]
    public class RegisterUserCommand : ContractModel, IRequest<OperationResponse<RegisterUserCommandResult>>
    {
        public NewUserDTO NewUser { get; set; }
    } 
}