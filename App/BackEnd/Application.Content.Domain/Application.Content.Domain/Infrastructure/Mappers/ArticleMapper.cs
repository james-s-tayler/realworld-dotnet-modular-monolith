using System;
using Application.Content.Domain.Contracts.DTOs;
using Application.Content.Domain.Entities;
using Application.Social.Domain.Contracts.DTOs;

namespace Application.Content.Domain.Infrastructure.Mappers
{
    internal static class ArticleMapper
    {
        internal static SingleArticleDTO ToArticleDTO(this Article article, string[] tagList, ProfileDTO author, bool isFavorited, int favoritesCount)
        {
            return new SingleArticleDTO
            {
                Slug = article.GetSlug(),
                Title = article.Title,
                Description = article.Description,
                Body = article.Body,
                CreatedAt = article.Created_At,
                UpdatedAt = article.Updated_At,
                TagList = tagList,
                Author = author,
                Favorited = isFavorited,
                FavoritesCount = favoritesCount
            };
        }
        
        internal static Article ToArticleEntity(this PublishArticleDTO article)
        {
            return new Article
            {
                Title = article.Title,
                Description = article.Description,
                Body = article.Body,
                TagList = article.TagList
            };
        }
    }
}