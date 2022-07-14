using System.ComponentModel.DataAnnotations;
using App.Content.Domain.Contracts.DTOs;
using App.Core.DataAccess;
using App.Core.PipelineBehaviors.OperationResponse;
using MediatR;

namespace App.Content.Domain.Contracts.Operations.Commands.EditArticle
{
    public class EditArticleCommand : ContractModel, IRequest<OperationResponse<EditArticleCommandResult>>
    {
        [Required]
        public string Slug { get; set; }
        [Required] 
        public EditArticleDTO UpdatedArticle { get; set; }
    }
}