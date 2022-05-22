using Destructurama.Attributed;
using TracerAttributes;

namespace Conduit.Identity.Domain.Contracts
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }
        public string Bio { get; set; }
        
        [NotLogged]
        public string Token { get; set; }
    }
}