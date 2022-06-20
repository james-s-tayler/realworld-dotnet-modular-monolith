using Application.Core.DataAccess;
using Application.Content.Domain.Contracts.DTOs;
using MediatR;

namespace Application.Content.Domain.Contracts.Operations.Commands.UpdateExample
{
    public class UpdateExampleCommandResult : ContractModel, INotification
    {
        public ExampleDTO ExampleOutput { get; set; }
    }
}