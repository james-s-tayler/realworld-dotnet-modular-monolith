using System.Threading.Tasks;
using Conduit.Users.Domain.Entities;

namespace Conduit.Users.Domain.Infrastructure.Services
{
    public interface IAuthTokenService
    {
        Task<string> GenerateAuthToken(User user);
    }
}