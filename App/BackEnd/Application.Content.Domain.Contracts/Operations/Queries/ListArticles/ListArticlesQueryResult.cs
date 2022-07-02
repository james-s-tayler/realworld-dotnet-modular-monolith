using Application.Content.Domain.Contracts.DTOs;
using Application.Core.DataAccess;

namespace Application.Content.Domain.Contracts.Operations.Queries.ListArticles
{
    public class ListArticlesQueryResult : ContractModel
    {
        public SingleArticleDTO Article { get; set; }
    }
}