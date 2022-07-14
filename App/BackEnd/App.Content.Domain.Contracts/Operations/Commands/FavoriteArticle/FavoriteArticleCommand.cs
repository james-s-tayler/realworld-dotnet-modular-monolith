using System.ComponentModel.DataAnnotations;
using App.Core.DataAccess;
using App.Core.PipelineBehaviors.OperationResponse;
using MediatR;

namespace App.Content.Domain.Contracts.Operations.Commands.FavoriteArticle
{
    public class FavoriteArticleCommand : ContractModel, IRequest<OperationResponse<FavoriteArticleCommandResult>>
    {
        [Required]
        public string Slug { get; set; }
    }
}