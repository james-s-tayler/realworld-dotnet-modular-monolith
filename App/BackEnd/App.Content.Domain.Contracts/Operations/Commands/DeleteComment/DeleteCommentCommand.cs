using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using App.Core.DataAccess;
using App.Core.PipelineBehaviors.OperationResponse;
using MediatR;

namespace App.Content.Domain.Contracts.Operations.Commands.DeleteComment
{
    [ExcludeFromCodeCoverage]
    public class DeleteCommentCommand : ContractModel, IRequest<OperationResponse<DeleteCommentCommandResult>>
    {
        [Required]
        public string ArticleSlug { get; set; }

        [Required]
        [Range(0, Int32.MaxValue)]
        public int CommentId { get; set; }
    }
}