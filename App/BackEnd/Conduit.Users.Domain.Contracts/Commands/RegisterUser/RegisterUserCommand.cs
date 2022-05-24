using Conduit.Core.DataAccess;
using Conduit.Core.PipelineBehaviors;
using Conduit.Core.PipelineBehaviors.Authorization;
using MediatR;

namespace Conduit.Users.Domain.Contracts.Commands.RegisterUser
{
    [AllowUnauthenticated]
    public class RegisterUserCommand : ContractModel, IRequest<OperationResponse<RegisterUserCommandResult>>
    {
        public NewUserDTO NewUser { get; set; }
    } 
}