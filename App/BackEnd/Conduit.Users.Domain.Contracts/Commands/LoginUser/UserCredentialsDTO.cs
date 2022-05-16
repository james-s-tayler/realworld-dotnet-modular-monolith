using Destructurama.Attributed;

namespace Conduit.Identity.Domain.Contracts.Commands.LoginUser
{
    public class UserCredentialsDTO
    {
        public string Email { get; set; }
        
        [NotLogged]
        public string Password { get; set; }
    }
}