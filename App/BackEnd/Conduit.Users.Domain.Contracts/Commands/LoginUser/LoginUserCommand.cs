using Conduit.Core.PipelineBehaviors;
using Conduit.Core.PipelineBehaviors.Authorization;
using MediatR;

namespace Conduit.Identity.Domain.Contracts.Commands.LoginUser
{
    [AllowUnauthenticated]
    public class LoginUserCommand : IRequest<OperationResponse<LoginUserResult>>
    {
        public UserCredentialsDTO UserCredentials { get; set; }
    }
}