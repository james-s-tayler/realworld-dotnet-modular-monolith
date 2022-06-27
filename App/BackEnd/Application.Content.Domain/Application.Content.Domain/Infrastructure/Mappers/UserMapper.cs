using Application.Content.Domain.Entities;
using Application.Users.Domain.Contracts.DTOs;

namespace Application.Content.Domain.Infrastructure.Mappers
{
    internal static class UserMapper
    {
        internal static UserEntity ToUser(this UserDTO user)
        {
            return new UserEntity
            {
                UserId = user.Id,
                Username = user.Username
            };
        }
    }
}