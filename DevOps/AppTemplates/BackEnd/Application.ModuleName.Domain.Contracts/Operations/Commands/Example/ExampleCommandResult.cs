using Application.Core.DataAccess;
using Application.ModuleName.Domain.Contracts.DTOs;
using MediatR;

namespace Application.ModuleName.Domain.Contracts.Operations.Commands.Example
{
    public class ExampleCommandResult : ContractModel, INotification
    {
        public ExampleDTO ExampleOutput { get; set; }
    }
}