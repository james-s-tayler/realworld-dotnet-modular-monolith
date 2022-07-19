using App.Core.DataAccess;

namespace App.Users.Domain.Contracts.DTOs
{
    public class ProfileDTO : ContractModel
    {
        public string Username { get; set; }
        public string Image { get; set; }
        public string Bio { get; set; }
        public bool Following { get; set; }
    }
}