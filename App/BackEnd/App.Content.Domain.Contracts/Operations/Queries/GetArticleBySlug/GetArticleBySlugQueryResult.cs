using App.Content.Domain.Contracts.DTOs;
using App.Core.DataAccess;

namespace App.Content.Domain.Contracts.Operations.Queries.GetArticleBySlug
{
    public class GetArticleBySlugQueryResult : ContractModel
    {
        public SingleArticleDTO Article { get; set; }
    }
}