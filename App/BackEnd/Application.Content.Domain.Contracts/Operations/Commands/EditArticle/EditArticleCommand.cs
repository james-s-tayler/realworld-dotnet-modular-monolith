using System.ComponentModel.DataAnnotations;
using Application.Content.Domain.Contracts.DTOs;
using Application.Core.DataAccess;
using Application.Core.PipelineBehaviors.OperationResponse;
using MediatR;

namespace Application.Content.Domain.Contracts.Operations.Commands.EditArticle
{
    public class EditArticleCommand : ContractModel, IRequest<OperationResponse<EditArticleCommandResult>>
    {
        [Required]
        public string Slug { get; set; }
        [Required] 
        public EditArticleDTO UpdatedArticle { get; set; }
    }
}