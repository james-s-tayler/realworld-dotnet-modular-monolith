using System.Threading.Tasks;
using Conduit.API.Models;
using Refit;

namespace Conduit.API.Tests.Integration
{
    public interface IConduitApiClient
    {
        [Post("/api/users")]
        Task<ApiResponse<UserResponse>> CreateUser([Body] NewUserRequest request);

        [Post("/api/articles")]
        Task<ApiResponse<SingleArticleResponse>> CreateArticle([Body] NewArticleRequest request);
        
        [Get("/api/articles/{slug}")]
        Task<ApiResponse<SingleArticleResponse>> GetArticle(string slug);
    }
}