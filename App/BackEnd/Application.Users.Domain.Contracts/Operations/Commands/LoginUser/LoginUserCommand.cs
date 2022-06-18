using Application.Core.DataAccess;
using Application.Core.PipelineBehaviors.Authorization;
using Application.Core.PipelineBehaviors.OperationResponse;
using Application.Users.Domain.Contracts.DTOs;
using MediatR;

namespace Application.Users.Domain.Contracts.Operations.Commands.LoginUser
{
    [AllowUnauthenticated]
    public class LoginUserCommand : ContractModel, IRequest<OperationResponse<LoginUserCommandResult>>
    {
        public UserCredentialsDTO UserCredentials { get; set; }
    }
}