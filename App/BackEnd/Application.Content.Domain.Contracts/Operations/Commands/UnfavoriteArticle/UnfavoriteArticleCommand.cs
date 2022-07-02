using System.ComponentModel.DataAnnotations;
using Application.Core.DataAccess;
using Application.Core.PipelineBehaviors.OperationResponse;
using MediatR;

namespace Application.Content.Domain.Contracts.Operations.Commands.UnfavoriteArticle
{
    public class UnfavoriteArticleCommand : ContractModel, IRequest<OperationResponse<UnfavoriteArticleCommandResult>>
    {
        [Required]
        public string Slug { get; set; }
    }
}