using Application.Content.Domain.Contracts.DTOs;
using Application.Content.Domain.Entities;

namespace Application.Content.Domain.Infrastructure.Mappers
{
    internal static class ArticleMapper
    {
        internal static SingleArticleDTO ToArticleDTO(this Article article)
        {
            return new SingleArticleDTO
            {
                //todo
            };
        }
        
        internal static Article ToArticleEntity(this PublishArticleDTO article)
        {
            return new Article
            {
                //todo
            };
        }
    }
}