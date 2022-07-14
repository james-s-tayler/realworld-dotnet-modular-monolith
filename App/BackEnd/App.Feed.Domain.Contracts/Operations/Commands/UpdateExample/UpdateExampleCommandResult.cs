using App.Core.DataAccess;
using App.Feed.Domain.Contracts.DTOs;
using MediatR;

namespace App.Feed.Domain.Contracts.Operations.Commands.UpdateExample
{
    public class UpdateExampleCommandResult : ContractModel, INotification
    {
        public ExampleDTO ExampleOutput { get; set; }
    }
}