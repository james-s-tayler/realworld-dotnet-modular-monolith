using Application.Core.DataAccess;
using Application.Core.PipelineBehaviors.Authorization;
using Application.Core.PipelineBehaviors.OperationResponse;
using MediatR;

namespace Conduit.Users.Domain.Contracts.Commands.LoginUser
{
    [AllowUnauthenticated]
    public class LoginUserCommand : ContractModel, IRequest<OperationResponse<LoginUserCommandResult>>
    {
        public UserCredentialsDTO UserCredentials { get; set; }
    }
}