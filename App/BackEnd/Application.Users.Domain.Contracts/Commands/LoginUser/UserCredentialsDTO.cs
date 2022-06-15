using Application.Core.DataAccess;
using Destructurama.Attributed;

namespace Application.Users.Domain.Contracts.Commands.LoginUser
{
    public class UserCredentialsDTO : ContractModel
    {
        public string Email { get; set; }
        
        [NotLogged]
        public string Password { get; set; }
    }
}