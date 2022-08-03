using App.Users.Domain.Contracts;
using App.Users.Domain.Contracts.DTOs;
using App.Users.Domain.Entities;

namespace App.Users.Domain.Infrastructure.Mappers
{
    internal static class UserMapper
    {
        internal static UserDTO ToUserDTO(this UserEntity userEntity, string token)
        {
            return new UserDTO
            {
                Id = userEntity.Id,
                Email = userEntity.Email,
                Username = userEntity.Username,
                Token = token,
                Image = userEntity.Image,
                Bio = userEntity.Bio
            };
        }

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