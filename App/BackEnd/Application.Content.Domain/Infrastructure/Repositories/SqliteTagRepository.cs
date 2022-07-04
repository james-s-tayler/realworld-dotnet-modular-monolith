using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Application.Content.Domain.Entities;
using Application.Content.Domain.Setup.Module;
using Application.Core.DataAccess;
using Dapper;
using JetBrains.Annotations;

namespace Application.Content.Domain.Infrastructure.Repositories
{
    internal class SqliteTagRepository : ITagRepository
    {
        private readonly DbConnection _connection;

        public SqliteTagRepository([NotNull] ModuleDbConnectionWrapper<ContentModule> connectionWrapper)
        {
            _connection = connectionWrapper.Connection;
        }

        public Task<List<TagEntity>> GetTags()
        {
            var articleTagsSql = "SELECT distinct(tag) FROM tags";
            var articleTags = _connection.Query<TagEntity>(articleTagsSql).ToList();
            return Task.FromResult(articleTags);
        }
        
        public Task<List<TagEntity>> GetByArticleId(int articleId)
        {
            var articleTagsSql = "SELECT * FROM tags t JOIN article_tags at ON at.tag_id = t.id WHERE at.article_id = @article_id";
            var articleTagsArguments = new { article_id = articleId };
            var articleTags = _connection.Query<TagEntity>(articleTagsSql, articleTagsArguments).ToList();
            return Task.FromResult(articleTags);
        }
    }
}