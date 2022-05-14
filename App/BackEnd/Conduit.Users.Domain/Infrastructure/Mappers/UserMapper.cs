using Conduit.Identity.Domain.Contracts;
using Conduit.Identity.Domain.Entities;

namespace Conduit.Identity.Domain.Infrastructure.Mappers
{
    internal static class UserMapper
    {
        internal static UserDTO ToUserDTO(this User user, string token)
        {
            return new UserDTO
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username,
                Token = token,
                Image = user.Image,
                Bio = user.Bio
            };
        }
    }
}