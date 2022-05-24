using Conduit.Core.DataAccess;

namespace Conduit.Users.Domain.Contracts.Commands.RegisterUser
{
    public class RegisterUserCommandResult : ContractModel
    {
        public UserDTO RegisteredUser { get; set; }
        public int UserId { get; set; }
    }
}