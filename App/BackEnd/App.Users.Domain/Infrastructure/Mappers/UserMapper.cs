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
    }
}