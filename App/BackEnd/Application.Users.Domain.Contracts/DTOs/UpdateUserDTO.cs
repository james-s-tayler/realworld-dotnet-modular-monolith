using Application.Core.DataAccess;

namespace Application.Users.Domain.Contracts.DTOs
{
    public class UpdateUserDTO : ContractModel
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Image { get; set; }
        public string Bio { get; set; }
    }
}