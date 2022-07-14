using System.Collections.Generic;
using App.Content.Domain.Contracts.DTOs;
using App.Core.DataAccess;

namespace App.Content.Domain.Contracts.Operations.Queries.ListArticles
{
    public class ListArticlesQueryResult : ContractModel
    {
        public List<SingleArticleDTO> Articles { get; set; }
    }
}