using System.Collections.Generic;
using System.Threading.Tasks;
using App.Content.Domain.Entities;

namespace App.Content.Domain.Infrastructure.Repositories
{
    internal interface IArticleRepository
    {
        Task Update(ArticleEntity articleEntity);
        Task<int> Create(ArticleEntity articleEntity);
        Task<IEnumerable<ArticleEntity>> GetUserFeed(int limit, int offset, int userId);
        Task<IEnumerable<ArticleEntity>> GetByFilters(string authorUsername, string favoritedByUsername, string tag, int limit, int offset, int? userId);
        Task<bool> ExistsBySlug(string slug);

        Task<ArticleEntity> GetById(int id, int? userId);
        Task<ArticleEntity> GetBySlug(string slug, int? userId);
        Task<IEnumerable<ArticleEntity>> GetAll(int? userId);
        Task Delete(int userId, string slug);
        Task FavoriteArticle(string slug, int userId);
        Task UnfavoriteArticle(string slug, int userId);
    }
}