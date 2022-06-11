using System.Threading.Tasks;
using Conduit.Users.Domain.Entities;

namespace Conduit.Users.Domain.Infrastructure.Services
{
    internal interface IAuthTokenService
    {
        Task<string> GenerateAuthToken(User user);
    }
}