using System;
using System.Collections.Generic;
using System.Linq;
using App.Content.Domain.Contracts.DTOs;

namespace Conduit.API.Models.Mappers
{
    public static class ArticleMapper
    {
        public static PublishArticleDTO ToPublishArticleDto(this NewArticle article)
        {
            return new PublishArticleDTO
            {
                Title = article.Title,
                Description = article.Description,
                Body = article.Body,
                TagList = article.TagList == null ? Array.Empty<string>() : article.TagList.ToArray()
            };
        }

        public static EditArticleDTO ToEditArticleDto(this UpdateArticleRequest article)
        {
            return new EditArticleDTO
            {
                Title = article.Article.Title,
                Description = article.Article.Description,
                Body = article.Article.Body
            };
        }

        public static SingleArticleResponse ToSingleArticleResponse(this SingleArticleDTO articleDto)
        {
            return new SingleArticleResponse
            {
                Article = articleDto.ToArticle()
            };
        }

        public static MultipleArticlesResponse ToMultipleArticlesResponse(this List<SingleArticleDTO> articleDtos)
        {
            var articles = new List<Article>();

            foreach ( var articleDto in articleDtos )
            {
                articles.Add(articleDto.ToArticle());
            }

            return new MultipleArticlesResponse
            {
                Articles = articles,
                ArticlesCount = articles.Count
            };
        }

        private static Article ToArticle(this SingleArticleDTO articleDto)
        {
            return new Article
            {
                Author = articleDto.Author.ToProfileResponse().Profile,
                Title = articleDto.Title,
                Slug = articleDto.Slug,
                Description = articleDto.Description,
                Body = articleDto.Body,
                Favorited = articleDto.Favorited,
                FavoritesCount = articleDto.FavoritesCount,
                TagList = articleDto.TagList == null ? new List<string>() : articleDto.TagList.ToList(),
                CreatedAt = articleDto.CreatedAt,
                UpdatedAt = articleDto.UpdatedAt
            };
        }
    }
}