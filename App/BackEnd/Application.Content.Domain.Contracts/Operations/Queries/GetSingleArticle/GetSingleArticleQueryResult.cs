using Application.Content.Domain.Contracts.DTOs;
using Application.Core.DataAccess;

namespace Application.Content.Domain.Contracts.Operations.Queries.GetSingleArticle
{
    public class GetSingleArticleQueryResult : ContractModel
    {
        public SingleArticleDTO Article { get; set; }
    }
}