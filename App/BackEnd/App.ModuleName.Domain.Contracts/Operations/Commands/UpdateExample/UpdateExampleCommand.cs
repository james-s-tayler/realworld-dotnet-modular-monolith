using System.ComponentModel.DataAnnotations;
using App.Core.DataAccess;
using App.Core.PipelineBehaviors.OperationResponse;
using App.ModuleName.Domain.Contracts.DTOs;
using MediatR;

namespace App.ModuleName.Domain.Contracts.Operations.Commands.UpdateExample
{
    //[AllowUnauthenticated] - for unauthorized requests
    public class UpdateExampleCommand : ContractModel, IRequest<OperationResponse<UpdateExampleCommandResult>>
    {
        [Required]
        public ExampleDTO ExampleInput { get; set; }
    } 
}