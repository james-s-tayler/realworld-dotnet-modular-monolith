using System.Threading.Tasks;
using Conduit.Identity.Domain.Entities;

namespace Conduit.Identity.Domain.Interactions.Inbound.Services
{
    public interface IAuthTokenService
    {
        Task<string> GenerateAuthToken(User user);
    }
}