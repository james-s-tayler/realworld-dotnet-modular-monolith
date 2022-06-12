using System.Threading.Tasks;

namespace Conduit.Users.Domain.Infrastructure.Services
{
    internal interface ISocialService
    {
        Task<bool> IsFollowing(int userId);
    }
}