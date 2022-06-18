using Application.Core.DataAccess;
using Application.Users.Domain.Contracts.DTOs;

namespace Application.Users.Domain.Contracts.Operations.Queries.GetCurrentUser
{
    public class GetCurrentUserQueryResult : ContractModel
    {
        public UserDTO CurrentUser { get; set; }
    }
}