using System.Threading.Tasks;
using Application.Users.Domain.Entities;

namespace Application.Users.Domain.Infrastructure.Services
{
    internal interface IAuthTokenService
    {
        Task<string> GenerateAuthToken(UserEntity userEntity);
    }
}