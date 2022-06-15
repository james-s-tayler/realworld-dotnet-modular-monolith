using Application.Core.DataAccess;

namespace Application.Users.Domain.Contracts.Queries.GetCurrentUser
{
    public class GetCurrentUserQueryResult : ContractModel
    {
        public UserDTO CurrentUser { get; set; }
    }
}