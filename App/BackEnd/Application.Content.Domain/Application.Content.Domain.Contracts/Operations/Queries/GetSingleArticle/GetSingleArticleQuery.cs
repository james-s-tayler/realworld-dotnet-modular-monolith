using System.ComponentModel.DataAnnotations;
using Application.Core.DataAccess;
using Application.Core.PipelineBehaviors.Authorization;
using Application.Core.PipelineBehaviors.OperationResponse;
using MediatR;

namespace Application.Content.Domain.Contracts.Operations.Queries.GetSingleArticle
{
    [AllowUnauthenticated]
    public class GetSingleArticleQuery : ContractModel, IRequest<OperationResponse<GetSingleArticleQueryResult>>
    {
        [Required]
        public string Slug { get; set; }
    }
}