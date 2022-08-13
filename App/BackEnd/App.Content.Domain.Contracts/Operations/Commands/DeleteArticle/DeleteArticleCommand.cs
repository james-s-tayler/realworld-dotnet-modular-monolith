using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using App.Core.DataAccess;
using App.Core.PipelineBehaviors.OperationResponse;
using MediatR;

namespace App.Content.Domain.Contracts.Operations.Commands.DeleteArticle
{
    [ExcludeFromCodeCoverage]
    public class DeleteArticleCommand : ContractModel, IRequest<OperationResponse<DeleteArticleCommandResult>>
    {
        [Required]
        public string Slug { get; set; }
    }
}