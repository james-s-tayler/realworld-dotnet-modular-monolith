using System.Threading;
using System.Threading.Tasks;
using Conduit.Core.PipelineBehaviors;
using Conduit.Identity.Domain.Contracts.Commands.UpdateUser;
using MediatR;

namespace Conduit.Identity.Domain.Operations.Commands.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, OperationResponse<UpdateUserResult>>
    {
        public Task<OperationResponse<UpdateUserResult>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(OperationResponseFactory.NotImplemented<UpdateUserCommand, OperationResponse<UpdateUserResult>>());
        }
    }
}