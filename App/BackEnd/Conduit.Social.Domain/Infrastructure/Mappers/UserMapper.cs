using Conduit.Social.Domain.Contracts;
using Conduit.Social.Domain.Entities;
using Conduit.Users.Domain.Contracts;

namespace Conduit.Social.Domain.Infrastructure.Mappers
{
    internal static class UserMapper
    {
        internal static User ToUser(this UserDTO user)
        {
            return new User
            {
                Id = user.Id,
                Username = user.Username,
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