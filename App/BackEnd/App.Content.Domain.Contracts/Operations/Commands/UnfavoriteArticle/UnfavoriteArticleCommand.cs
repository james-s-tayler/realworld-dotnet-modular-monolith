using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using App.Core.DataAccess;
using App.Core.PipelineBehaviors.OperationResponse;
using MediatR;

namespace App.Content.Domain.Contracts.Operations.Commands.UnfavoriteArticle
{
    [ExcludeFromCodeCoverage]
    public class UnfavoriteArticleCommand : ContractModel, IRequest<OperationResponse<UnfavoriteArticleCommandResult>>
    {
        [Required]
        public string Slug { get; set; }
    }
}