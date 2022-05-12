using Conduit.Core.Validation;
using MediatR;

namespace Conduit.Identity.Domain.Contracts.RegisterUser
{
    public class RegisterUserCommand : IRequest<OperationResponse<RegisterUserResult>>
    {
        public NewUserDTO NewUser { get; set; }
    } 
}