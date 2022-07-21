using System.Collections.Generic;
using App.Content.Domain.Contracts.DTOs;
using App.Core.DataAccess;
using MediatR;

namespace App.Feed.Domain.Contracts.Operations.Queries.GetFeed
{
    public class GetFeedQueryResult : ContractModel, INotification
    {
        public List<SingleArticleDTO> FeedArticles { get; set; }
    }
}