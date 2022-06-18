using Application.Core.DataAccess;

namespace Application.Social.Domain.Contracts.Commands.UnfollowUser
{
    public class UnfollowUserCommandResult : ContractModel
    {
        public ProfileDTO UnfollowedProfile { get; set; }
    }
}