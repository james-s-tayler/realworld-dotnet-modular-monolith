using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using App.Core.DataAccess;
using App.Core.PipelineBehaviors.OperationResponse;
using App.ModuleName.Domain.Contracts.DTOs;
using MediatR;

namespace App.ModuleName.Domain.Contracts.Operations.Commands.UpdateExample
{
    [ExcludeFromCodeCoverage]
    //[AllowUnauthenticated] - for unauthorized requests
    public class UpdateExampleCommand : ContractModel, IRequest<OperationResponse<UpdateExampleCommandResult>>
    {
        [Required]
        public ExampleDTO ExampleInput { get; set; }
    }
}