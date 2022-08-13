using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using App.Content.Domain.Contracts.DTOs;
using App.Core.DataAccess;

namespace App.Content.Domain.Contracts.Operations.Queries.ListFeedArticles
{
    [ExcludeFromCodeCoverage]
    public class ListFeedArticlesQueryResult : ContractModel
    {
        public List<SingleArticleDTO> Articles { get; set; }
    }
}