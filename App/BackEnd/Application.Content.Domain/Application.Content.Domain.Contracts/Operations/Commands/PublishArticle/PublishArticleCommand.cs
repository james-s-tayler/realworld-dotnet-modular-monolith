using System.ComponentModel.DataAnnotations;
using Application.Content.Domain.Contracts.DTOs;
using Application.Core.DataAccess;
using Application.Core.PipelineBehaviors.OperationResponse;
using MediatR;

namespace Application.Content.Domain.Contracts.Operations.Commands.PublishArticle
{
    public class PublishArticleCommand : ContractModel, IRequest<OperationResponse<PublishArticleCommandResult>>
    {
        [Required]
        public PublishArticleDTO NewArticle { get; set; }
    }
}