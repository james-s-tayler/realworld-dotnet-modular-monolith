using Conduit.Core.PipelineBehaviors;
using MediatR;

namespace Conduit.Identity.Domain.Contracts.Commands.UpdateUser
{
    public class UpdateUserCommand : IRequest<OperationResponse<UpdateUserResult>>
    {
        public UpdateUserDTO UpdateUser { get; set; }
    }
}