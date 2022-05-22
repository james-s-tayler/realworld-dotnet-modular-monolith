using TracerAttributes;

namespace Conduit.Identity.Domain.Contracts.Commands.UpdateUser
{
    public class UpdateUserDTO
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Image { get; set; }
        public string Bio { get; set; }
    }
}