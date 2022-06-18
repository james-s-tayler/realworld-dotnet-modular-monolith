using Application.Core.DataAccess;
using Destructurama.Attributed;

namespace Application.Users.Domain.Contracts.DTOs
{
    public class NewUserDTO : ContractModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        
        [NotLogged]
        public string Password { get; set; }
    }
}