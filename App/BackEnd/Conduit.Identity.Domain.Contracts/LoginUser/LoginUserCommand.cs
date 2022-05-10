using Conduit.Core.Validation;
using MediatR;

namespace Conduit.Identity.Domain.Contracts.LoginUser
{
    public class LoginUserCommand : IRequest<OperationResponse<LoginUserResult>>
    {
        public UserCredentialsDTO UserCredentials { get; set; }
    }
}