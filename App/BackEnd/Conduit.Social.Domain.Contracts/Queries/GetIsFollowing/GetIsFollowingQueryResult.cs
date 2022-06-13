using Conduit.Core.DataAccess;

namespace Conduit.Social.Domain.Contracts.Queries.GetIsFollowing
{
    public class GetIsFollowingQueryResult : ContractModel
    {
        public bool Following { get; set; }
    }
}