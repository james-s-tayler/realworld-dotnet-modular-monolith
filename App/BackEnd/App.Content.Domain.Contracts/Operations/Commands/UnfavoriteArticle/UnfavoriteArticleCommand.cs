using System.ComponentModel.DataAnnotations;
using App.Core.DataAccess;
using App.Core.PipelineBehaviors.OperationResponse;
using MediatR;

namespace App.Content.Domain.Contracts.Operations.Commands.UnfavoriteArticle
{
    public class UnfavoriteArticleCommand : ContractModel, IRequest<OperationResponse<UnfavoriteArticleCommandResult>>
    {
        [Required]
        public string Slug { get; set; }
    }
}