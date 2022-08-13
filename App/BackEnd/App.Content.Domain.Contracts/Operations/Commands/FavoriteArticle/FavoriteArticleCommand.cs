using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using App.Core.DataAccess;
using App.Core.PipelineBehaviors.OperationResponse;
using MediatR;

namespace App.Content.Domain.Contracts.Operations.Commands.FavoriteArticle
{
    [ExcludeFromCodeCoverage]
    public class FavoriteArticleCommand : ContractModel, IRequest<OperationResponse<FavoriteArticleCommandResult>>
    {
        [Required]
        public string Slug { get; set; }
    }
}