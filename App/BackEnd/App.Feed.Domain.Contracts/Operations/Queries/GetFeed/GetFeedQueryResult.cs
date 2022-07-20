using App.Core.DataAccess;
using App.Feed.Domain.Contracts.DTOs;
using MediatR;

namespace App.Feed.Domain.Contracts.Operations.Queries.GetFeed
{
    public class GetFeedQueryResult : ContractModel, INotification
    {
        public ExampleDTO ExampleOutput { get; set; }
    }
}