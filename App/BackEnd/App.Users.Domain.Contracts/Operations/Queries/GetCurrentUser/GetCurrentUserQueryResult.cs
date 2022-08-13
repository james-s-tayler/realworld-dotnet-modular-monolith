using System.Diagnostics.CodeAnalysis;
using App.Core.DataAccess;
using App.Users.Domain.Contracts.DTOs;

namespace App.Users.Domain.Contracts.Operations.Queries.GetCurrentUser
{
    [ExcludeFromCodeCoverage]
    public class GetCurrentUserQueryResult : ContractModel
    {
        public UserDTO CurrentUser { get; set; }
    }
}