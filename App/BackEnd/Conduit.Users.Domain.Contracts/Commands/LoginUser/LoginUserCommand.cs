using Conduit.Core.DataAccess;
using Conduit.Core.PipelineBehaviors;
using Conduit.Core.PipelineBehaviors.Authorization;
using MediatR;

namespace Conduit.Users.Domain.Contracts.Commands.LoginUser
{
    [AllowUnauthenticated]
    public class LoginUserCommand : ContractModel, IRequest<OperationResponse<LoginUserResult>>
    {
        public UserCredentialsDTO UserCredentials { get; set; }
    }
}