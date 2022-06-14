using System.Threading.Tasks;
using Conduit.API.Models;
using Refit;

namespace Conduit.API.Tests.Integration
{
    public interface IConduitApiClient
    {
        [Post("/api/users")]
        Task<ApiResponse<UserResponse>> CreateUser([Body] NewUserRequest request);
    }
}