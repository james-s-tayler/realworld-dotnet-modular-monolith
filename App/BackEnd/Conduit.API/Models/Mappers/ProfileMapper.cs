using App.Users.Domain.Contracts.DTOs;

namespace Conduit.API.Models.Mappers
{
    /// <summary>
    /// 
    /// </summary>
    public static class ProfileMapper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="profileDto"></param>
        /// <returns></returns>
        public static ProfileResponse ToProfileResponse(this ProfileDTO profileDto)
        {
            return new ProfileResponse
            {
                Profile = new Profile
                {
                    Username = profileDto.Username,
                    Image = profileDto.Image,
                    Bio = profileDto.Bio,
                    Following = profileDto.Following
                }
            };
        }
    }
}