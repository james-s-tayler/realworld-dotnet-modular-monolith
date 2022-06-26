using Application.Users.Domain.Contracts;
using Application.Users.Domain.Contracts.DTOs;
using Application.Users.Domain.Entities;

namespace Application.Users.Domain.Infrastructure.Mappers
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