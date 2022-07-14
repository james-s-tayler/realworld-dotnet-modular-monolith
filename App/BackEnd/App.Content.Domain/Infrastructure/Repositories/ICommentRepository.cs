using System.Collections.Generic;
using System.Threading.Tasks;
using App.Content.Domain.Entities;

namespace App.Content.Domain.Infrastructure.Repositories
{
    internal interface ICommentRepository
    {
        Task<bool> ExistsBySlugAndId(string articleSlug, int commentId);
        Task<CommentEntity> PostComment(CommentEntity comment);
        Task DeleteComment(string articleSlug, int commentId);
        Task<List<CommentEntity>> GetCommentsByArticleId(int articleId);
    }
}