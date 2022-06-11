using Conduit.Core.DataAccess;
using Conduit.Core.PipelineBehaviors;
using Conduit.Core.PipelineBehaviors.Authorization;
using Conduit.Core.PipelineBehaviors.OperationResponse;
using MediatR;

namespace Conduit.Users.Domain.Contracts.Commands.LoginUser
{
    [AllowUnauthenticated]
    public class LoginUserCommand : ContractModel, IRequest<OperationResponse<LoginUserCommandResult>>
    {
        public UserCredentialsDTO UserCredentials { get; set; }
    }
}