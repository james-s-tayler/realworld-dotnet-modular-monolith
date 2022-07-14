using App.Core.DataAccess;
using App.Social.Domain.Contracts.DTOs;

namespace App.Social.Domain.Contracts.Operations.Queries.GetProfile
{
    public class GetProfileQueryResult : ContractModel
    {
        public ProfileDTO Profile { get; set; }
    }
}