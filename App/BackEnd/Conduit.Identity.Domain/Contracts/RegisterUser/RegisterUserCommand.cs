using MediatR;

namespace Conduit.Identity.Domain.Contracts.RegisterUser
{
    public class RegisterUserCommand : IRequest<RegisterUserResult>
    {
        public NewUserDTO NewUser { get; set; }
    } 
}