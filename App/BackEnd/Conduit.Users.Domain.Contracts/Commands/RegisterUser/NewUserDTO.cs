using Application.Core.DataAccess;
using Destructurama.Attributed;

namespace Conduit.Users.Domain.Contracts.Commands.RegisterUser
{
    public class NewUserDTO : ContractModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        
        [NotLogged]
        public string Password { get; set; }
    }
}