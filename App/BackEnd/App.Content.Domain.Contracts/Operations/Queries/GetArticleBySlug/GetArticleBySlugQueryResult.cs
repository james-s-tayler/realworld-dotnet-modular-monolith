using System.Diagnostics.CodeAnalysis;
using App.Content.Domain.Contracts.DTOs;
using App.Core.DataAccess;

namespace App.Content.Domain.Contracts.Operations.Queries.GetArticleBySlug
{
    [ExcludeFromCodeCoverage]
    public class GetArticleBySlugQueryResult : ContractModel
    {
        public SingleArticleDTO Article { get; set; }
    }
}