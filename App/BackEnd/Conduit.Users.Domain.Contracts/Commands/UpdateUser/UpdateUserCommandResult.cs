using Conduit.Core.DataAccess;

namespace Conduit.Users.Domain.Contracts.Commands.UpdateUser
{
    public class UpdateUserCommandResult : ContractModel
    {
        public UserDTO UpdatedUser { get; set; }
    }
}