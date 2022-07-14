using App.Core.DataAccess;
using App.Social.Domain.Contracts.DTOs;
using MediatR;

namespace App.Social.Domain.Contracts.Operations.Commands.UnfollowUser
{
    public class UnfollowUserCommandResult : ContractModel, INotification
    {
        public ProfileDTO UnfollowedProfile { get; set; }
    }
}