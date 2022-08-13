using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using App.Core.DataAccess;
using App.Core.PipelineBehaviors.OperationResponse;
using MediatR;

namespace App.Users.Domain.Contracts.Operations.Commands.FollowUser
{
    [ExcludeFromCodeCoverage]
    public class FollowUserCommand : ContractModel, IRequest<OperationResponse<FollowUserCommandResult>>
    {
        [Required]
        public string Username { get; set; }
    }
}