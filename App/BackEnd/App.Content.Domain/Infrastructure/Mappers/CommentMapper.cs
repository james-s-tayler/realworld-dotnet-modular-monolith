using App.Content.Domain.Contracts.DTOs;
using App.Content.Domain.Entities;
using App.Users.Domain.Contracts;
using App.Users.Domain.Contracts.DTOs;

namespace App.Content.Domain.Infrastructure.Mappers
{
    internal static class CommentMapper
    {
        internal static SingleCommentDTO ToCommentDTO(this CommentEntity commentEntity, ProfileDTO commenter)
        {
            return new SingleCommentDTO
            {
                Id = commentEntity.Id,
                Body = commentEntity.Body,
                CreatedAt = commentEntity.CreatedAt,
                UpdatedAt = commentEntity.UpdatedAt,
                Author = commenter
            };
        }
        
        internal static CommentEntity ToComment(this PostCommentDTO comment, ArticleEntity article)
        {
            return new CommentEntity
            {
                Body = comment.Body,
                ArticleId = article.Id
            };
        }
    }
}