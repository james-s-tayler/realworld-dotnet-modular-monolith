using System.Threading.Tasks;
using App.Users.Domain.Entities;

namespace App.Users.Domain.Infrastructure.Services
{
    internal interface IAuthTokenService
    {
        Task<string> GenerateAuthToken(UserEntity userEntity);
    }
}