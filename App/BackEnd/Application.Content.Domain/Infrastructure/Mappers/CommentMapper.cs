using System;
using System.Linq;
using Application.Content.Domain.Contracts.DTOs;
using Application.Content.Domain.Entities;
using Application.Social.Domain.Contracts.DTOs;

namespace Application.Content.Domain.Infrastructure.Mappers
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