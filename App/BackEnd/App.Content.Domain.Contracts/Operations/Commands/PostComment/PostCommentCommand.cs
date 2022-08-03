using System.ComponentModel.DataAnnotations;
using App.Content.Domain.Contracts.DTOs;
using App.Core.DataAccess;
using App.Core.PipelineBehaviors.OperationResponse;
using MediatR;

namespace App.Content.Domain.Contracts.Operations.Commands.PostComment
{
    public class PostCommentCommand : ContractModel, IRequest<OperationResponse<PostCommentCommandResult>>
    {
        [Required]
        public string ArticleSlug { get; set; }

        [Required]
        public PostCommentDTO NewComment { get; set; }
    }
}