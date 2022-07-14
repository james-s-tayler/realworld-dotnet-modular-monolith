using App.Social.Domain.Contracts.DTOs;

namespace Conduit.API.Models.Mappers
{
    public static class ProfileMapper
    {
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