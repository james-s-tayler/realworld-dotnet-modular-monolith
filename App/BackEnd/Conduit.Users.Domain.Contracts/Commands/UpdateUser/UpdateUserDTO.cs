using Application.Core.DataAccess;

namespace Conduit.Users.Domain.Contracts.Commands.UpdateUser
{
    public class UpdateUserDTO : ContractModel
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Image { get; set; }
        public string Bio { get; set; }
    }
}