using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using App.Content.Domain.Contracts.DTOs;
using App.Core.DataAccess;
using App.Core.PipelineBehaviors.OperationResponse;
using MediatR;

namespace App.Content.Domain.Contracts.Operations.Commands.PublishArticle
{
    [ExcludeFromCodeCoverage]
    public class PublishArticleCommand : ContractModel, IRequest<OperationResponse<PublishArticleCommandResult>>
    {
        [Required]
        public PublishArticleDTO NewArticle { get; set; }
    }
}