using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Application.Core.DataAccess;
using Application.Content.Domain.Entities;
using Application.Content.Domain.Setup.Module;
using Dapper;
using JetBrains.Annotations;

namespace Application.Content.Domain.Infrastructure.Repositories
{
    internal class SqliteArticleRepository : IArticleRepository
    {
        private readonly DbConnection _connection;

        public SqliteArticleRepository([NotNull] ModuleDbConnectionWrapper<ContentModule> connectionWrapper)
        {
            _connection = connectionWrapper.Connection;
        }

        public Task<bool> Exists(int id)
        {
            string sql = "SELECT EXISTS(SELECT 1 FROM articles WHERE id=@id)";

            var arguments = new
            {
                id = id
            };

            var exists = _connection.ExecuteScalar<bool>(sql, arguments);

            return Task.FromResult(exists);
        }
        
        public Task<bool> ExistsBySlug(string slug)
        {
            string sql = "SELECT EXISTS(SELECT 1 FROM articles WHERE slug=@slug)";

            var arguments = new { slug };

            var exists = _connection.ExecuteScalar<bool>(sql, arguments);

            return Task.FromResult(exists);
        }

        public Task<Article> GetBySlug(string slug)
        {
            string sql = "SELECT * FROM articles WHERE slug=@slug";

            var arguments = new { slug };

            var articles = _connection.Query<Article>(sql, arguments);

            return Task.FromResult(articles.SingleOrDefault());
        }

        public Task<Article> GetById(int id)
        {
            string sql = "SELECT * FROM articles WHERE id=@id";

            var arguments = new { id };

            var articles = _connection.Query<Article>(sql, arguments);

            return Task.FromResult(articles.SingleOrDefault());
        }

        public Task<IEnumerable<Article>> GetAll()
        {
            string sql = "SELECT * FROM articles";

            return Task.FromResult(_connection.Query<Article>(sql));
        }

        public Task<int> Create([NotNull] Article article)
        {
            var sql = "INSERT INTO articles (user_id, slug, title, description, body, created_at, updated_at) VALUES (@user_id, @slug, @title, @description, @body, @created_at, @updated_at) RETURNING *";

            var now = DateTime.UtcNow;
            
            var arguments = new
            {
                user_id = article.UserId, 
                slug = article.GetSlug(), 
                title = article.Title, 
                description = article.Description, 
                body = article.Body, 
                created_at = now.ToString("O"), 
                updated_at = now.ToString("O")
            };

            var insertedArticle = _connection.QuerySingle<Article>(sql, arguments);

            return Task.FromResult(insertedArticle.Id);
        }

        public Task Update([NotNull] Article article)
        {
            var sql = "UPDATE articles SET slug = @slug, title = @title, description = @description, body = @body, updated_at = @updated_at WHERE id = @id";

            var arguments = new
            {
                slug = article.GetSlug(), 
                title = article.Title, 
                description = article.Description, 
                body = article.Body,
                updated_at = DateTime.UtcNow.ToString("O")
            };

            _connection.Execute(sql, arguments);
            return Task.CompletedTask;
        }

        public Task Delete(int id)
        {
            var sql = "DELETE FROM articles WHERE id = @id";

            var arguments = new { id };

            _connection.Execute(sql, arguments);
            return Task.CompletedTask;
        }

        public Task<int> DeleteAll()
        {
            var sql = "DELETE FROM articles";

            return Task.FromResult(_connection.Execute(sql));
        }
    }
}