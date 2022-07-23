using System.ComponentModel.DataAnnotations;
using App.Core.DataAccess;
using App.Core.PipelineBehaviors.Authorization;
using App.Core.PipelineBehaviors.OperationResponse;
using MediatR;

namespace App.Content.Domain.Contracts.Operations.Queries.GetArticleBySlug
{
    [AllowUnauthenticated]
    public class GetArticleBySlugQuery : ContractModel, IRequest<OperationResponse<GetArticleBySlugQueryResult>>
    {
        [Required]
        public string Slug { get; set; }
    }
}