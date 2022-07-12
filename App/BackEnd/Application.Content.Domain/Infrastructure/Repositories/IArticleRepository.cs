using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Core.DataAccess;
using Application.Content.Domain.Entities;

namespace Application.Content.Domain.Infrastructure.Repositories
{
    internal interface IArticleRepository : ICrudRepository<ArticleEntity, int>
    {
        Task<IEnumerable<ArticleEntity>> GetUserFeed(int limit, int offset);
        Task<IEnumerable<ArticleEntity>> GetByFilters(string authorUsername, string favoritedByUsername, string tag, int limit, int offset);
        Task<bool> ExistsBySlug(string slug);
        Task<ArticleEntity> GetBySlug(string slug);
        Task FavoriteArticle(string slug);
        Task UnfavoriteArticle(string slug);
    }
}