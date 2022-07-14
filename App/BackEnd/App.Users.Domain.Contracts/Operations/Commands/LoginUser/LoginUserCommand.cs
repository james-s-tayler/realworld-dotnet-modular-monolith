using App.Core.DataAccess;
using App.Core.PipelineBehaviors.Authorization;
using App.Core.PipelineBehaviors.OperationResponse;
using App.Users.Domain.Contracts.DTOs;
using MediatR;

namespace App.Users.Domain.Contracts.Operations.Commands.LoginUser
{
    [AllowUnauthenticated]
    public class LoginUserCommand : ContractModel, IRequest<OperationResponse<LoginUserCommandResult>>
    {
        public UserCredentialsDTO UserCredentials { get; set; }
    }
}