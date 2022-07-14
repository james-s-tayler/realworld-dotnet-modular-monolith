using System.Collections.Generic;
using System.Linq;
using App.Content.Domain.Contracts.DTOs;

namespace Conduit.API.Models.Mappers
{
    public static class CommentMapper
    {
        public static SingleCommentResponse ToSingleCommentResponse(this SingleCommentDTO commentDto)
        {
            return new SingleCommentResponse
            {
                Comment = commentDto.ToComment()
            };
        }
        
        public static MultipleCommentsResponse ToMultipleCommentsResponse(this List<SingleCommentDTO> commentDtos)
        {
            var comments = new List<Comment>();

            foreach (var commentDto in commentDtos)
            {
                comments.Add(commentDto.ToComment());
            }
            
            return new MultipleCommentsResponse
            {
                Comments = comments
            };
        }
        
        private static Comment ToComment(this SingleCommentDTO commentDto)
        {
            return new Comment
            {
                Author = commentDto.Author.ToProfileResponse().Profile,
                Body = commentDto.Body,
                CreatedAt = commentDto.CreatedAt,
                UpdatedAt = commentDto.UpdatedAt
            };
        }
    }
}