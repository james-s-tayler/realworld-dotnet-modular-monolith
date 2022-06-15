using Conduit.Core.DataAccess;

namespace Conduit.Social.Domain.Contracts.Commands.UnfollowUser
{
    public class UnfollowUserCommandResult : ContractModel
    {
        public ProfileDTO UnfollowedProfile { get; set; }
    }
}