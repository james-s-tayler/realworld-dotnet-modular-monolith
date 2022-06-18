using Application.Core.DataAccess;
using Application.Social.Domain.Contracts.DTOs;
using MediatR;

namespace Application.Social.Domain.Contracts.Operations.Commands.FollowUser
{
    public class FollowUserCommandResult : ContractModel, INotification
    {
        public ProfileDTO FollowedProfile { get; set; }
    }
}