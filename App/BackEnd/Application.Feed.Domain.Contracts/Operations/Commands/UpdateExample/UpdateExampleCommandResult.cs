using Application.Core.DataAccess;
using Application.Feed.Domain.Contracts.DTOs;
using MediatR;

namespace Application.Feed.Domain.Contracts.Operations.Commands.UpdateExample
{
    public class UpdateExampleCommandResult : ContractModel, INotification
    {
        public ExampleDTO ExampleOutput { get; set; }
    }
}