using App.Core.DataAccess;
using Application.ModuleName.Domain.Contracts.DTOs;
using MediatR;

namespace Application.ModuleName.Domain.Contracts.Operations.Commands.UpdateExample
{
    public class UpdateExampleCommandResult : ContractModel, INotification
    {
        public ExampleDTO ExampleOutput { get; set; }
    }
}