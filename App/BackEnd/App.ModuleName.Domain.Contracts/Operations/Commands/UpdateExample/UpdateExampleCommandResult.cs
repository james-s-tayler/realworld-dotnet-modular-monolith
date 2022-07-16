using App.Core.DataAccess;
using App.ModuleName.Domain.Contracts.DTOs;
using MediatR;

namespace App.ModuleName.Domain.Contracts.Operations.Commands.UpdateExample
{
    public class UpdateExampleCommandResult : ContractModel, INotification
    {
        public ExampleDTO ExampleOutput { get; set; }
    }
}