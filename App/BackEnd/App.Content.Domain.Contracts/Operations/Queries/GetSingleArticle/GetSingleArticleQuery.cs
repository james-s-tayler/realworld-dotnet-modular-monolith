using System.ComponentModel.DataAnnotations;
using App.Core.DataAccess;
using App.Core.PipelineBehaviors.Authorization;
using App.Core.PipelineBehaviors.OperationResponse;
using MediatR;

namespace App.Content.Domain.Contracts.Operations.Queries.GetSingleArticle
{
    [AllowUnauthenticated]
    public class GetSingleArticleQuery : ContractModel, IRequest<OperationResponse<GetSingleArticleQueryResult>>
    {
        [Required]
        public string Slug { get; set; }
    }
}