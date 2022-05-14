using Conduit.Core.PipelineBehaviors;
using Conduit.Core.PipelineBehaviors.Authorization;
using MediatR;

namespace Conduit.Identity.Domain.Contracts.Commands.RegisterUser
{
    [AllowUnauthenticated]
    public class RegisterUserCommand : IRequest<OperationResponse<RegisterUserResult>>
    {
        public NewUserDTO NewUser { get; set; }
    } 
}