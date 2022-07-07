using System;
using System.ComponentModel.DataAnnotations;
using Application.Core.DataAccess;
using Application.Core.PipelineBehaviors.OperationResponse;
using MediatR;

namespace Application.Content.Domain.Contracts.Operations.Commands.DeleteComment
{
    public class DeleteCommentCommand : ContractModel, IRequest<OperationResponse<DeleteCommentCommandResult>>
    {
        [Required]
        public string ArticleSlug { get; set; }
        
        [Required]
        [Range(0, Int32.MaxValue)]
        public int CommentId { get; set; }
    }
}