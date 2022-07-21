using App.Core.DataAccess;
using App.Users.Domain.Contracts.DTOs;
using MediatR;

namespace App.Users.Domain.Contracts.Operations.Commands.FollowUser
{
    public class FollowUserCommandResult : ContractModel, INotification
    {
        public int UserId { get; set; }
        public int FollowingUserId { get; set; }
        public ProfileDTO FollowedProfile { get; set; }
    }
}