using System.Collections.Generic;
using App.Content.Domain.Contracts.DTOs;

namespace Conduit.API.Models.Mappers
{
    /// <summary>
    /// 
    /// </summary>
    public static class CommentMapper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="commentDto"></param>
        /// <returns></returns>
        public static SingleCommentResponse ToSingleCommentResponse(this SingleCommentDTO commentDto)
        {
            return new SingleCommentResponse
            {
                Comment = commentDto.ToComment()
            };
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="commentDtos"></param>
        /// <returns></returns>
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
                Id = commentDto.Id,
                Author = commentDto.Author.ToProfileResponse().Profile,
                Body = commentDto.Body,
                CreatedAt = commentDto.CreatedAt,
                UpdatedAt = commentDto.UpdatedAt
            };
        }
    }
}