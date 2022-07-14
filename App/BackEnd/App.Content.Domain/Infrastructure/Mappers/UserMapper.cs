using App.Content.Domain.Entities;
using App.Users.Domain.Contracts.DTOs;

namespace App.Content.Domain.Infrastructure.Mappers
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