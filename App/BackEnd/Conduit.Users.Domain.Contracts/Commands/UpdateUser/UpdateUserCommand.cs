using Conduit.Core.PipelineBehaviors;
using MediatR;
using TracerAttributes;

namespace Conduit.Identity.Domain.Contracts.Commands.UpdateUser
{
    public class UpdateUserCommand : IRequest<OperationResponse<UpdateUserResult>>
    {
        public UpdateUserDTO UpdateUser { get; set; }
    }
}