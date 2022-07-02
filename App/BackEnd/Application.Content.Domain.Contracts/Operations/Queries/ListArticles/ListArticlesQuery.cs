using System.ComponentModel.DataAnnotations;
using Application.Core.DataAccess;
using Application.Core.PipelineBehaviors.Authorization;
using Application.Core.PipelineBehaviors.OperationResponse;
using MediatR;

namespace Application.Content.Domain.Contracts.Operations.Queries.ListArticles
{
    [AllowUnauthenticated]
    public class ListArticlesQuery : ContractModel, IRequest<OperationResponse<ListArticlesQueryResult>>
    {
        [Required]
        public string Slug { get; set; }
    }
}