using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Content.Domain.Entities;

namespace Application.Content.Domain.Infrastructure.Repositories
{
    internal interface ITagRepository
    {
        Task<bool> Exists(string tag);
        Task<List<TagEntity>> GetTags();
        Task<List<TagEntity>> GetByArticleId(int articleId);
    }
}