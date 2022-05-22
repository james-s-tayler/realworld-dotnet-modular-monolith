using Destructurama.Attributed;
using TracerAttributes;

namespace Conduit.Identity.Domain.Contracts.Commands.RegisterUser
{
    public class NewUserDTO
    {
        public string Username { get; set; }
        public string Email { get; set; }
        
        [NotLogged]
        public string Password { get; set; }
    }
}