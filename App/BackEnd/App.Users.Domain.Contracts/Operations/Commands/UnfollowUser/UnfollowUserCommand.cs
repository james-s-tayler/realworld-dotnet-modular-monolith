using System.ComponentModel.DataAnnotations;
using App.Core.DataAccess;
using App.Core.PipelineBehaviors.OperationResponse;
using MediatR;

namespace App.Users.Domain.Contracts.Operations.Commands.UnfollowUser
{
    public class UnfollowUserCommand : ContractModel, IRequest<OperationResponse<UnfollowUserCommandResult>>
    {
        [Required]
        public string Username { get; set; }
    }
}