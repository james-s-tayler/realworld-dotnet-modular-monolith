using Conduit.Core.DataAccess;

namespace Conduit.Users.Domain.Contracts.Commands.UpdateUser
{
    public class UpdateUserResult : ContractModel
    {
        public UserDTO UpdatedUser { get; set; }
    }
}