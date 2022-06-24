using System.Threading.Tasks;
using Application.Core.DataAccess;
using Application.Content.Domain.Entities;

namespace Application.Content.Domain.Infrastructure.Repositories
{
    internal interface IArticleRepository : ICrudRepository<Article, int>
    {
        Task<bool> ExistsBySlug(string slug);
        Task<Article> GetBySlug(string slug);
    }
}