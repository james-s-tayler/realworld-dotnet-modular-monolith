using TracerAttributes;

namespace Conduit.Identity.Domain.Contracts.Commands.RegisterUser
{
    public class RegisterUserResult
    {
        public UserDTO RegisteredUser { get; set; }
        public int UserId { get; set; }
    }
}