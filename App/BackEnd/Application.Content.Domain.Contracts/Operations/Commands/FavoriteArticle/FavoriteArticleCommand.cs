using System.ComponentModel.DataAnnotations;
using Application.Core.DataAccess;
using Application.Core.PipelineBehaviors.OperationResponse;
using MediatR;

namespace Application.Content.Domain.Contracts.Operations.Commands.FavoriteArticle
{
    public class FavoriteArticleCommand : ContractModel, IRequest<OperationResponse<FavoriteArticleCommandResult>>
    {
        [Required]
        public string Slug { get; set; }
    }
}