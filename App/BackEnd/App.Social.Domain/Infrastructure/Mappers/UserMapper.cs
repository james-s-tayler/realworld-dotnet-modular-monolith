using App.Social.Domain.Contracts.DTOs;
using App.Social.Domain.Entities;
using App.Users.Domain.Contracts.DTOs;

namespace App.Social.Domain.Infrastructure.Mappers
{
    internal static class UserMapper
    {
        internal static UserEntity ToUser(this UserDTO user)
        {
            return new UserEntity
            {
                Id = user.Id,
                Username = user.Username,
                Image = user.Image,
                Bio = user.Bio
            };
        }
        
        internal static ProfileDTO ToProfileDTO(this UserEntity userEntity, bool isFollowing)
        {
            return new ProfileDTO
            {
                Username = userEntity.Username,
                Image = userEntity.Image,
                Bio = userEntity.Bio,
                Following = isFollowing
            };
        }
    }
}