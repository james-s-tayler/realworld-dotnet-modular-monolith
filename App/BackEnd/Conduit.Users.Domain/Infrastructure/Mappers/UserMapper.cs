using Conduit.Users.Domain.Contracts;
using Conduit.Users.Domain.Entities;

namespace Conduit.Users.Domain.Infrastructure.Mappers
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
        
        internal static ProfileDTO ToProfileDTO(this User user, bool isFollowing)
        {
            return new ProfileDTO
            {
                Username = user.Username,
                Image = user.Image,
                Bio = user.Bio,
                Following = isFollowing
            };
        }
    }
}