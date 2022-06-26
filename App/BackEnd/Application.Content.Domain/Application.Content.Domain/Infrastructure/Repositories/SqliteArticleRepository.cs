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
        private readonly ITagRepository _tagRepository;
        private readonly DbConnection _connection;

        public SqliteArticleRepository([NotNull] ModuleDbConnectionWrapper<ContentModule> connectionWrapper, 
            ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
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

        public async Task<ArticleEntity> GetBySlug(string slug)
        {
            string sql = "SELECT * FROM articles WHERE slug=@slug";

            var arguments = new { slug };

            var article = _connection.QuerySingleOrDefault<ArticleEntity>(sql, arguments);
            if (article != null)
            {
                article.TagList = await _tagRepository.GetByArticleId(article.Id);
            }

            return article;
        }

        public async Task<ArticleEntity> GetById(int id)
        {
            string sql = "SELECT * FROM articles WHERE id=@id";

            var arguments = new { id };

            var article = _connection.QuerySingleOrDefault<ArticleEntity>(sql, arguments);
            if (article != null)
            {
                article.TagList = await _tagRepository.GetByArticleId(article.Id);
            }

            return article;
        }

        public async Task<IEnumerable<ArticleEntity>> GetAll()
        {
            string sql = "SELECT * FROM articles";
            var articles = _connection.Query<ArticleEntity>(sql).ToList();

            foreach (var article in articles)
            {
                article.TagList = await _tagRepository.GetByArticleId(article.Id);
            }
            
            return articles;
        }

        public Task<int> Create([NotNull] ArticleEntity articleEntity)
        {
            var sql = "INSERT INTO articles (user_id, slug, title, description, body, created_at, updated_at) VALUES (@user_id, @slug, @title, @description, @body, @created_at, @updated_at) RETURNING *";

            var now = DateTime.UtcNow;
            
            var arguments = new
            {
                user_id = articleEntity.UserId, 
                slug = articleEntity.GetSlug(), 
                title = articleEntity.Title, 
                description = articleEntity.Description, 
                body = articleEntity.Body, 
                created_at = now.ToString("O"), 
                updated_at = now.ToString("O")
            };

            var insertedArticle = _connection.QuerySingle<ArticleEntity>(sql, arguments);

            foreach (var tag in articleEntity.TagList.Select(tag => tag.Tag))
            {
                var getTagIdSql = "SELECT id FROM tags WHERE tag = @tag";
                var getTagIdArguments = new { tag };
                var tagEntity = _connection.QuerySingleOrDefault<TagEntity>(getTagIdSql, getTagIdArguments);

                if (tagEntity == null)
                {
                    var insertTagSql = "INSERT INTO tags (tag) VALUES (@tag) RETURNING *";
                    var insertTagArguments = new { tag };
                    tagEntity = _connection.QuerySingle<TagEntity>(insertTagSql, insertTagArguments);
                }

                var insertArticleTagSql = "INSERT OR IGNORE INTO article_tags (article_id, tag_id) VALUES (@article_id, @tag_id)";
                var insertArticleTagArguments = new { article_id = insertedArticle.Id, tag_id = tagEntity.Id };
                _connection.Execute(insertArticleTagSql, insertArticleTagArguments);
            }

            return Task.FromResult(insertedArticle.Id);
        }

        public Task Update([NotNull] ArticleEntity articleEntity)
        {
            var sql = "UPDATE articles SET slug = @slug, title = @title, description = @description, body = @body, updated_at = @updated_at WHERE id = @id";

            var arguments = new
            {
                slug = articleEntity.GetSlug(), 
                title = articleEntity.Title, 
                description = articleEntity.Description, 
                body = articleEntity.Body,
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