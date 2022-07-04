using System.ComponentModel.DataAnnotations;
using Application.Content.Domain.Contracts.DTOs;
using Application.Core.DataAccess;
using Application.Core.PipelineBehaviors.OperationResponse;
using MediatR;

namespace Application.Content.Domain.Contracts.Operations.Commands.DeleteArticle
{
    public class DeleteArticleCommand : ContractModel, IRequest<OperationResponse<DeleteArticleCommandResult>>
    {
        [Required]
        public string Slug { get; set; }
    }
}