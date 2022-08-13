using System.Diagnostics.CodeAnalysis;
using App.Core.DataAccess;
using Destructurama.Attributed;

namespace App.Users.Domain.Contracts.DTOs
{
    [ExcludeFromCodeCoverage]
    public class UserDTO : ContractModel
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