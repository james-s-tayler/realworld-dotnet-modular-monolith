using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using App.Content.Domain.Entities;
using App.Content.Domain.Setup.Module;
using App.Core.Context;
using App.Core.DataAccess;
using Dapper;
using JetBrains.Annotations;

namespace App.Content.Domain.Infrastructure.Repositories
{
    internal class SqliteCommentRepository : ICommentRepository
    {
        private readonly DbConnection _connection;

        public SqliteCommentRepository([NotNull] ModuleDbConnectionWrapper<ContentModule> connectionWrapper)
        {
            _connection = connectionWrapper.Connection;
        }

        public Task<bool> ExistsBySlugAndId(string articleSlug, int commentId)
        {
            var sql = "SELECT EXISTS(SELECT 1 FROM comments comment JOIN articles article ON article.id = comment.article_id WHERE article.slug=@article_slug AND comment.id = @comment_id)";
            var arguments = new
            {
                article_slug = articleSlug,
                comment_id = commentId
            };
            return Task.FromResult(_connection.ExecuteScalar<bool>(sql, arguments));
        }

        public Task<CommentEntity> PostComment(UserEntity commentAuthor, CommentEntity comment)
        {
            string sql = "INSERT INTO comments (user_id, article_id, body, created_at, updated_at) VALUES (@user_id, @article_id, @body, @created_at, @updated_at) RETURNING *";

            var now = DateTime.UtcNow.ToString("O");

            var arguments = new
            {
                user_id = commentAuthor.UserId,
                article_id = comment.ArticleId,
                body = comment.Body,
                created_at = now,
                updated_at = now
            };

            comment = _connection.QuerySingle<CommentEntity>(sql, arguments);
            comment.Author = commentAuthor;

            return Task.FromResult(comment);
        }

        public Task DeleteComment(string articleSlug, int commentId)
        {
            /*DELETE FROM TR_ContactResultRecord
              WHERE ROWID IN (
                SELECT a.ROWID FROM TR_ContactResultRecord a
                INNER JOIN TR_Case b
                  ON (a.FireStationCode=b.FireStationCode and a.CaseNo=b.CaseCode )
                WHERE b.Update_DateTime <=20140628134416
              );*/
            /*string sql = "DELETE FROM comments comment " +
                         "JOIN articles article ON article.id = comment.article_id " +
                         "WHERE article.slug=@article_slug " +
                         "AND comment.id = @comment_id";*/

            string sql = "DELETE FROM comments " +
                         "WHERE ROWID IN (" +
                           "SELECT comment.ROWID from comments comment " +
                           "JOIN articles article ON article.id = comment.article_id " +
                           "WHERE article.slug=@article_slug " +
                           "AND comment.id = @comment_id" +
                         ")";

            var arguments = new
            {
                article_slug = articleSlug,
                comment_id = commentId
            };

            _connection.Execute(sql, arguments);
            return Task.CompletedTask;
        }

        public Task<List<CommentEntity>> GetCommentsByArticleSlug(string slug)
        {
            var sql = "SELECT comment.*, user.* " +
                      "FROM comments comment " +
                      "JOIN users user ON user.user_id = comment.user_id " +
                      "JOIN articles article ON article.id = comment.article_id " +
                      "WHERE article.slug=@slug";

            var arguments = new { slug };

            var comments = _connection.Query<CommentEntity, UserEntity, CommentEntity>(sql,
                (comment, user) =>
                {
                    comment.Author = user;
                    return comment;
                },
                splitOn: "id, user_id",
                param: arguments);

            return Task.FromResult(comments.ToList());
        }
    }
}