using System.ComponentModel.DataAnnotations;
using Application.Core.DataAccess;
using Application.Core.PipelineBehaviors.Authorization;
using Application.Core.PipelineBehaviors.OperationResponse;
using MediatR;

namespace Application.Content.Domain.Contracts.Operations.Queries.GetArticleComments
{
    [AllowUnauthenticated]
    public class GetArticleCommentsQuery : ContractModel, IRequest<OperationResponse<GetArticleCommentsQueryResult>>
    {
        [Required]
        public string Slug { get; set; }
    }
}