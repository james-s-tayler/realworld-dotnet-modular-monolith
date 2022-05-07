using Conduit.Core.Validation;
using MediatR;

namespace Conduit.Identity.Domain.Contracts.RegisterUser
{
    public class RegisterUserCommand : IRequest<ValidateableResponse<RegisterUserResult>>, IValidateable
    {
        public NewUserDTO NewUser { get; set; }
    } 
}