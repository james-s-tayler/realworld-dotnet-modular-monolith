using Application.Social.Domain.Contracts;
using Application.Social.Domain.Entities;
using Application.Users.Domain.Contracts;

namespace Application.Social.Domain.Infrastructure.Mappers
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