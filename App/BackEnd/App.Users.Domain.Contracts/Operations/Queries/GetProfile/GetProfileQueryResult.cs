using App.Core.DataAccess;
using App.Users.Domain.Contracts.DTOs;

namespace App.Users.Domain.Contracts.Operations.Queries.GetProfile
{
    public class GetProfileQueryResult : ContractModel
    {
        public ProfileDTO Profile { get; set; }
    }
}