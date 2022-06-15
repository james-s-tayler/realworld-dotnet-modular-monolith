using Conduit.Core.DataAccess;

namespace Conduit.Social.Domain.Contracts.Commands.FollowUser
{
    public class FollowUserCommandResult : ContractModel
    {
        public ProfileDTO FollowedProfile { get; set; }
    }
}