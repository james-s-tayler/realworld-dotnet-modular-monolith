using Application.Core.DataAccess;

namespace Application.Social.Domain.Contracts.Queries.GetProfile
{
    public class GetProfileQueryResult : ContractModel
    {
        public ProfileDTO Profile { get; set; }
    }
}