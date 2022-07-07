using System.ComponentModel.DataAnnotations;
using Application.Content.Domain.Contracts.DTOs;
using Application.Core.DataAccess;
using Application.Core.PipelineBehaviors.OperationResponse;
using MediatR;

namespace Application.Content.Domain.Contracts.Operations.Commands.PostComment
{
    public class PostCommentCommand : ContractModel, IRequest<OperationResponse<PostCommentCommandResult>>
    {
        [Required]
        public string ArticleSlug { get; set; }
        
        [Required]
        public PostCommentDTO NewComment { get; set; }
    }
}