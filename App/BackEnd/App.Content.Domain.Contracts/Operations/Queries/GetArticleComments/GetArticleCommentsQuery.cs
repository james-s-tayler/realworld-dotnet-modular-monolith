using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using App.Core.DataAccess;
using App.Core.PipelineBehaviors.Authorization;
using App.Core.PipelineBehaviors.OperationResponse;
using MediatR;

namespace App.Content.Domain.Contracts.Operations.Queries.GetArticleComments
{
    [ExcludeFromCodeCoverage]
    [AllowUnauthenticated]
    public class GetArticleCommentsQuery : ContractModel, IRequest<OperationResponse<GetArticleCommentsQueryResult>>
    {
        [Required]
        public string Slug { get; set; }
    }
}