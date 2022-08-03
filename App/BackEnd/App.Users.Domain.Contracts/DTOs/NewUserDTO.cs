using App.Core.DataAccess;
using Destructurama.Attributed;

namespace App.Users.Domain.Contracts.DTOs
{
    public class NewUserDTO : ContractModel
    {
        public string Username { get; set; }
        public string Email { get; set; }

        [NotLogged]
        public string Password { get; set; }
    }
}