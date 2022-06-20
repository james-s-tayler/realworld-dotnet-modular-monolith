using Application.Core.DataAccess;
using Application.Core.PipelineBehaviors.OperationResponse;
using Application.Content.Domain.Contracts.DTOs;
using MediatR;

namespace Application.Content.Domain.Contracts.Operations.Commands.UpdateExample
{
    //[AllowUnauthenticated] - for unauthorized requests
    public class UpdateExampleCommand : ContractModel, IRequest<OperationResponse<UpdateExampleCommandResult>>
    {
        public ExampleDTO ExampleInput { get; set; }
    }
}