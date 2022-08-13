using System.Diagnostics.CodeAnalysis;
using App.Core.DataAccess;
using App.Users.Domain.Contracts.DTOs;
using MediatR;

namespace App.Users.Domain.Contracts.Operations.Commands.UnfollowUser
{
    [ExcludeFromCodeCoverage]
    public class UnfollowUserCommandResult : ContractModel, INotification
    {
        public int UserId { get; set; }
        public int FollowingUserId { get; set; }
        public ProfileDTO UnfollowedProfile { get; set; }
    }
}