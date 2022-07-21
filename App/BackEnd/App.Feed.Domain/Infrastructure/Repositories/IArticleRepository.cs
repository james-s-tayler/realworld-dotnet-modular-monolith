using System.Threading.Tasks;
using App.Feed.Domain.Entities;

namespace App.Feed.Domain.Infrastructure.Repositories
{
    internal interface IArticleRepository
    {
        Task<bool> Exists(int articleId);
        Task<ArticleEntity> Insert(ArticleEntity publishedArticle);
    }
}