using System.Collections.Generic;
using Application.Content.Domain.Contracts.DTOs;
using Application.Core.DataAccess;

namespace Application.Content.Domain.Contracts.Operations.Queries.ListFeedArticles
{
    public class ListFeedArticlesQueryResult : ContractModel
    {
        public List<SingleArticleDTO> Articles { get; set; }
    }
}