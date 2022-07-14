using App.Content.Domain.Contracts.DTOs;
using App.Core.DataAccess;

namespace App.Content.Domain.Contracts.Operations.Queries.GetSingleArticle
{
    public class GetSingleArticleQueryResult : ContractModel
    {
        public SingleArticleDTO Article { get; set; }
    }
}