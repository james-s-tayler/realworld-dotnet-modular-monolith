using Application.Core.DataAccess;
using Application.Core.PipelineBehaviors.OperationResponse;
using Application.ModuleName.Domain.Contracts.DTOs;
using MediatR;

namespace Application.ModuleName.Domain.Contracts.Operations.Commands.Example
{
    //[AllowUnauthenticated] - for unauthorized requests
    public class ExampleCommand : ContractModel, IRequest<OperationResponse<ExampleCommandResult>>
    {
        public ExampleDTO ExampleInput { get; set; }
    } 
}