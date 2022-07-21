using System.Data.Common;
using System.Threading.Tasks;
using App.Core.DataAccess;
using App.Feed.Domain.Entities;
using App.Feed.Domain.Setup.Module;
using Dapper;
using JetBrains.Annotations;

namespace App.Feed.Domain.Infrastructure.Repositories
{
    internal class FeedArticleRepository : IArticleRepository
    {
        private readonly DbConnection _connection;

        public FeedArticleRepository([NotNull] ModuleDbConnectionWrapper<FeedModule> connectionWrapper)
        {
            _connection = connectionWrapper.Connection;
        }

        public Task<bool> Exists(int articleId)
        {
            var sql = "SELECT EXISTS (SELECT 1 FROM articles WHERE article_id=@article_id)";
            var arguments = new
            {
                article_id = articleId
            };
            var exists = _connection.ExecuteScalar<bool>(sql, arguments);
            return Task.FromResult(exists);
        }

        public Task<ArticleEntity> Insert(ArticleEntity publishedArticle)
        {
            var sql = "INSERT OR IGNORE INTO articles (article_id, user_id, created_at) VALUES (@article_id, @user_id, @created_at) RETURNING *";

            var arguments = new
            {
                article_id = publishedArticle.ArticleId,
                user_id = publishedArticle.UserId,
                created_at = publishedArticle.CreatedAt.ToString("O")
            };
            
            var insertedArticle = _connection.QuerySingle<ArticleEntity>(sql, arguments);
            
            return Task.FromResult(insertedArticle);
        }
    }
}