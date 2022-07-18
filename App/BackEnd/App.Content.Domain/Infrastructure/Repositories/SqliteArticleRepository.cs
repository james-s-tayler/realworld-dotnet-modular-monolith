using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using App.Content.Domain.Entities;
using App.Content.Domain.Setup.Module;
using App.Core.DataAccess;
using Dapper;
using JetBrains.Annotations;

namespace App.Content.Domain.Infrastructure.Repositories
{
    internal class SqliteArticleRepository : IArticleRepository
    {
        private readonly ITagRepository _tagRepository;
        private readonly IUserRepository _userRepository;
        private readonly DbConnection _connection;

        public SqliteArticleRepository([NotNull] ModuleDbConnectionWrapper<ContentModule> connectionWrapper, 
            [NotNull] ITagRepository tagRepository,
            [NotNull] IUserRepository userRepository)
        {
            _tagRepository = tagRepository;
            _userRepository = userRepository;
            _connection = connectionWrapper.Connection;
        }

        public Task<bool> ExistsBySlug(string slug)
        {
            string sql = "SELECT EXISTS(SELECT 1 FROM articles WHERE slug=@slug)";

            var arguments = new { slug };

            var exists = _connection.ExecuteScalar<bool>(sql, arguments);

            return Task.FromResult(exists);
        }

        private async Task EnrichArticle(ArticleEntity article, int? userId)
        {
            //un-optimized n+1 query
            if (article != null)
            {
                article.Author = await _userRepository.GetByArticleId(article.Id);
                article.TagList = await _tagRepository.GetByArticleId(article.Id);

                if (userId.HasValue)
                {
                    var favoritedSql = "SELECT EXISTS(SELECT 1 FROM article_favorites WHERE article_id=@article_id AND user_id=@user_id)";
                    var favoritedArguments = new { article_id = article.Id, user_id = userId };
                    article.Favorited = _connection.ExecuteScalar<bool>(favoritedSql, favoritedArguments);    
                }
                else
                {
                    article.Favorited = false;
                }
                
                var favoritesCountSql = "SELECT COUNT(*) FROM article_favorites WHERE article_id=@article_id AND user_id=@user_id";
                var favoritesCountArguments = new { article_id = article.Id, user_id = article.Author.UserId };
                article.FavoritesCount = _connection.ExecuteScalar<int>(favoritesCountSql, favoritesCountArguments);
            }
        }

        public async Task<ArticleEntity> GetBySlug(string slug, int? userId)
        {
            string sql = "SELECT * FROM articles a WHERE slug=@slug";

            var arguments = new { slug };

            var article = _connection.QuerySingleOrDefault<ArticleEntity>(sql, arguments);
            await EnrichArticle(article, userId);

            return article;
        }

        public async Task<ArticleEntity> GetById(int id, int? userId)
        {
            string sql = "SELECT * FROM articles WHERE id=@id";

            var arguments = new { id };

            var article = _connection.QuerySingleOrDefault<ArticleEntity>(sql, arguments);
            await EnrichArticle(article, userId);

            return article;
        }
        
        public async Task<IEnumerable<ArticleEntity>> GetAll(int? userId)
        {
            string sql = "SELECT * FROM articles";
            var articles = _connection.Query<ArticleEntity>(sql).ToList();

            foreach (var article in articles)
            {
                await EnrichArticle(article, userId);
            }
            
            return articles;
        }

        public async Task<IEnumerable<ArticleEntity>> GetUserFeed(int limit, int offset, int userId)
        {
            string sql = "SELECT a.* FROM articles a " +
                         "JOIN users author ON author.user_id = a.user_id " +
                         "LEFT JOIN article_tags at ON at.article_id = a.id " +
                         "LEFT JOIN tags t ON t.id = at.tag_id " +
                         "LEFT JOIN article_favorites af ON af.article_id = a.id " +
                         "LEFT JOIN users favoriter ON favoriter.user_id = af.user_id " +
                         "WHERE (@author_username IS NULL OR author.username = @author_username) " +
                         "AND (@favorited_by_username IS NULL OR favoriter.username = @favorited_by_username) " +
                         "AND (@tag IS NULL OR t.tag = @tag) " +
                         "LIMIT @limit " + //limit and offset is not efficient in SQLite
                         "OFFSET @offset"; //limit and offset is not efficient in SQLite
            
            var arguments = new
            {
                limit = limit,
                offset = offset
            };
            
            var articles = _connection.Query<ArticleEntity>(sql, arguments).ToList();

            foreach (var article in articles)
            {
                await EnrichArticle(article, userId);
            }
            
            return articles;
        }

        public async Task<IEnumerable<ArticleEntity>> GetByFilters(string authorUsername, string favoritedByUsername, string tag, int limit, int offset, int? userId)
        {
            string sql = "SELECT a.* FROM articles a " +
                         "JOIN users author ON author.user_id = a.user_id " +
                         "LEFT JOIN article_tags at ON at.article_id = a.id " +
                         "LEFT JOIN tags t ON t.id = at.tag_id " +
                         "LEFT JOIN article_favorites af ON af.article_id = a.id " +
                         "LEFT JOIN users favoriter ON favoriter.user_id = af.user_id " +
                         "WHERE (@author_username IS NULL OR author.username = @author_username) " +
                         "AND (@favorited_by_username IS NULL OR favoriter.username = @favorited_by_username) " +
                         "AND (@tag IS NULL OR t.tag = @tag) " +
                         "LIMIT @limit " + //limit and offset is not efficient in SQLite
                         "OFFSET @offset"; //limit and offset is not efficient in SQLite
            
            var arguments = new
            {
                author_username = authorUsername,
                favorited_by_username = favoritedByUsername,
                tag = tag,
                limit = limit,
                offset = offset
            };
            
            var articles = _connection.Query<ArticleEntity>(sql, arguments).ToList();

            foreach (var article in articles)
            {
                await EnrichArticle(article, userId);
            }
            
            return articles;
        }
        
        public Task FavoriteArticle(string slug, int userId)
        {
            var sql = "INSERT OR IGNORE INTO article_favorites (article_id, user_id) VALUES ((SELECT id FROM articles WHERE slug=@slug), @user_id)";
            var arguments = new { slug, user_id = userId };
            _connection.Execute(sql, arguments);
            
            return Task.CompletedTask;
        }

        public Task UnfavoriteArticle(string slug, int userId)
        {
            var sql = "DELETE FROM article_favorites WHERE article_id=(SELECT id FROM articles WHERE slug=@slug) AND user_id=@user_id";
            var arguments = new { slug, user_id = userId };
            _connection.Execute(sql, arguments);
            
            return Task.CompletedTask;
        }

        public Task<int> Create([NotNull] ArticleEntity articleEntity)
        {
            var sql = "INSERT INTO articles (user_id, slug, title, description, body, created_at, updated_at) VALUES (@user_id, @slug, @title, @description, @body, @created_at, @updated_at) RETURNING *";

            var now = DateTime.UtcNow;

            var arguments = new
            {
                user_id = articleEntity.Author.UserId,
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
                id = articleEntity.Id,
                slug = articleEntity.GetSlug(),
                title = articleEntity.Title, 
                description = articleEntity.Description, 
                body = articleEntity.Body,
                updated_at = DateTime.UtcNow.ToString("O")
            };

            _connection.Execute(sql, arguments);
            return Task.CompletedTask;
        }

        public Task Delete(int userId, string slug)
        {
            var selectArticleIdSql = "SELECT id FROM articles WHERE slug=@slug";
            var selectArticleIdArguments = new { slug };
            var id = _connection.ExecuteScalar<int>(selectArticleIdSql, selectArticleIdArguments);
            
            var deleteArticleTagsSql = "DELETE FROM article_tags WHERE article_id = @article_id";
            var deleteArticleTagsArguments = new { article_id = id };
            _connection.Execute(deleteArticleTagsSql, deleteArticleTagsArguments);
            
            var deleteArticleFavoritesSql = "DELETE FROM article_favorites WHERE article_id = @article_id AND user_id=@user_id";
            var deleteArticleFavoritesArguments = new { article_id = id, user_id = userId };
            _connection.Execute(deleteArticleFavoritesSql, deleteArticleFavoritesArguments);

            var sql = "DELETE FROM articles WHERE id = @id";
            var arguments = new { id };
            _connection.Execute(sql, arguments);
            
            return Task.CompletedTask;
        }
    }
}