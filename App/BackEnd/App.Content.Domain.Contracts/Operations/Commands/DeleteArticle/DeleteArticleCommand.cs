using System.ComponentModel.DataAnnotations;
using App.Core.DataAccess;
using App.Core.PipelineBehaviors.OperationResponse;
using MediatR;

namespace App.Content.Domain.Contracts.Operations.Commands.DeleteArticle
{
    public class DeleteArticleCommand : ContractModel, IRequest<OperationResponse<DeleteArticleCommandResult>>
    {
        [Required]
        public string Slug { get; set; }
    }
}