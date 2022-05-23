using Conduit.Core.DataAccess;

namespace Conduit.Users.Domain.Contracts.Queries.GetCurrentUser
{
    public class GetCurrentUserQueryResult : ContractModel
    {
        public UserDTO CurrentUser { get; set; }
    }
}