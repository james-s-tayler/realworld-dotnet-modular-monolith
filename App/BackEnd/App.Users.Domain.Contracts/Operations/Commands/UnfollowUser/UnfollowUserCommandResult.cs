using App.Core.DataAccess;
using App.Users.Domain.Contracts.DTOs;
using MediatR;

namespace App.Users.Domain.Contracts.Operations.Commands.UnfollowUser
{
    public class UnfollowUserCommandResult : ContractModel, INotification
    {
        public ProfileDTO UnfollowedProfile { get; set; }
    }
}