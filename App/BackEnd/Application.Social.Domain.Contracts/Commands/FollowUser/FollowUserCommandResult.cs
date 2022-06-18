using Application.Core.DataAccess;

namespace Application.Social.Domain.Contracts.Commands.FollowUser
{
    public class FollowUserCommandResult : ContractModel
    {
        public ProfileDTO FollowedProfile { get; set; }
    }
}