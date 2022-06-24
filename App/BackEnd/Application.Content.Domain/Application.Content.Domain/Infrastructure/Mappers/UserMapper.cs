using Application.Content.Domain.Entities;
using Application.Users.Domain.Contracts.DTOs;

namespace Application.Content.Domain.Infrastructure.Mappers
{
    internal static class UserMapper
    {
        internal static User ToUser(this UserDTO user)
        {
            return new User
            {
                Id = user.Id,
                Username = user.Username
            };
        }
    }
}