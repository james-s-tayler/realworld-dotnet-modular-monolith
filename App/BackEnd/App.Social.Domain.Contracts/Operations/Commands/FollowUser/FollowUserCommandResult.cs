using App.Core.DataAccess;
using App.Social.Domain.Contracts.DTOs;
using MediatR;

namespace App.Social.Domain.Contracts.Operations.Commands.FollowUser
{
    public class FollowUserCommandResult : ContractModel, INotification
    {
        public ProfileDTO FollowedProfile { get; set; }
    }
}