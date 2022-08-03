using System.Linq;
using App.Content.Domain.Contracts.DTOs;
using App.Content.Domain.Entities;
using App.Users.Domain.Contracts;
using App.Users.Domain.Contracts.DTOs;

namespace App.Content.Domain.Infrastructure.Mappers
{
    internal static class ArticleMapper
    {
        internal static SingleArticleDTO ToArticleDTO(this ArticleEntity articleEntity, ProfileDTO author)
        {
            return new SingleArticleDTO
            {
                Slug = articleEntity.GetSlug(),
                Title = articleEntity.Title,
                Description = articleEntity.Description,
                Body = articleEntity.Body,
                CreatedAt = articleEntity.CreatedAt,
                UpdatedAt = articleEntity.UpdatedAt,
                TagList = articleEntity.TagList.Select(tag => tag.Tag).ToArray(),
                Author = author,
                Favorited = articleEntity.Favorited,
                FavoritesCount = articleEntity.FavoritesCount
            };
        }

        internal static ArticleEntity ToArticleEntity(this PublishArticleDTO article, UserEntity author)
        {
            return new ArticleEntity
            {
                Author = author,
                Title = article.Title,
                Description = article.Description,
                Body = article.Body,
                TagList = article.TagList.Select(tag => new TagEntity { Tag = tag }).ToList()
            };
        }
    }
}