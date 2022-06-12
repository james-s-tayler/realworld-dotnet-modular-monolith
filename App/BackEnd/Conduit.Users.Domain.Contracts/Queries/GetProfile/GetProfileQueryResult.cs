using Conduit.Core.DataAccess;

namespace Conduit.Users.Domain.Contracts.Queries.GetProfile
{
    public class GetProfileQueryResult : ContractModel
    {
        public ProfileDTO Profile { get; set; }
    }
}