using Application.Core.DataAccess;
using Application.Social.Domain.Contracts.DTOs;
using MediatR;

namespace Application.Social.Domain.Contracts.Operations.Commands.UnfollowUser
{
    public class UnfollowUserCommandResult : ContractModel, INotification
    {
        public ProfileDTO UnfollowedProfile { get; set; }
    }
}