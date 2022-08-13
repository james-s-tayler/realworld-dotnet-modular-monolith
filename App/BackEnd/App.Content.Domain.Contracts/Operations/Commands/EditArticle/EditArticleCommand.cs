using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using App.Content.Domain.Contracts.DTOs;
using App.Core.DataAccess;
using App.Core.PipelineBehaviors.OperationResponse;
using MediatR;

namespace App.Content.Domain.Contracts.Operations.Commands.EditArticle
{
    [ExcludeFromCodeCoverage]
    public class EditArticleCommand : ContractModel, IRequest<OperationResponse<EditArticleCommandResult>>
    {
        [Required]
        public string Slug { get; set; }
        [Required]
        public EditArticleDTO UpdatedArticle { get; set; }
    }
}