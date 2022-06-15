/*
 * Conduit API
 *
 * Conduit API
 *
 * The version of the OpenAPI document: 1.0.0
 * 
 * Generated by: https://openapi-generator.tech
 */

using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Conduit.API.Attributes;
using Conduit.API.Models;
using Conduit.API.Models.Mappers;
using Conduit.Core.PipelineBehaviors.OperationResponse;
using Conduit.Social.Domain.Contracts.Commands.FollowUser;
using Conduit.Social.Domain.Contracts.Commands.UnfollowUser;
using Conduit.Social.Domain.Contracts.Queries.GetProfile;
using MediatR;

namespace Conduit.API.Controllers
{ 
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class ProfileApiController : OperationResponseController
    {

        public ProfileApiController(IMediator mediator) : base(mediator) {}
        
        /// <summary>
        /// Follow a user
        /// </summary>
        /// <remarks>Follow a user by username</remarks>
        /// <param name="username">Username of the profile you want to follow</param>
        /// <response code="200">OK</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="422">Unexpected error</response>
        [HttpPost]
        [Route("/api/profiles/{username}/follow")]
        [ValidateModelState]
        [SwaggerOperation("FollowUserByUsername")]
        [SwaggerResponse(statusCode: 200, type: typeof(ProfileResponse), description: "OK")]
        [SwaggerResponse(statusCode: 422, type: typeof(GenericErrorModel), description: "Unexpected error")]
        public virtual async Task<IActionResult> FollowUserByUsername([FromRoute (Name = "username")][Required]string username)
        {
            var followUserResponse = await Mediator.Send(new FollowUserCommand { Username = username });
            
            if (followUserResponse.Result != OperationResult.Success)
                return UnsuccessfulResponseResult(followUserResponse);

            return Ok(followUserResponse.Response.FollowedProfile.ToProfileResponse());
        }

        /// <summary>
        /// Get a profile
        /// </summary>
        /// <remarks>Get a profile of a user of the system. Auth is optional</remarks>
        /// <param name="username">Username of the profile to get</param>
        /// <response code="200">OK</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="422">Unexpected error</response>
        [HttpGet]
        [AllowAnonymous]
        [Route("/api/profiles/{username}")]
        [ValidateModelState]
        [SwaggerOperation("GetProfileByUsername")]
        [SwaggerResponse(statusCode: 200, type: typeof(ProfileResponse), description: "OK")]
        [SwaggerResponse(statusCode: 422, type: typeof(GenericErrorModel), description: "Unexpected error")]
        public virtual async Task<IActionResult> GetProfileByUsername([FromRoute (Name = "username")][Required]string username)
        {
            var getProfileResponse = await Mediator.Send(new GetProfileQuery { Username = username });
            
            if (getProfileResponse.Result != OperationResult.Success)
                return UnsuccessfulResponseResult(getProfileResponse);

            return Ok(getProfileResponse.Response.Profile.ToProfileResponse());
        }

        /// <summary>
        /// Unfollow a user
        /// </summary>
        /// <remarks>Unfollow a user by username</remarks>
        /// <param name="username">Username of the profile you want to unfollow</param>
        /// <response code="200">OK</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="422">Unexpected error</response>
        [HttpDelete]
        [Route("/api/profiles/{username}/follow")]
        [ValidateModelState]
        [SwaggerOperation("UnfollowUserByUsername")]
        [SwaggerResponse(statusCode: 200, type: typeof(ProfileResponse), description: "OK")]
        [SwaggerResponse(statusCode: 422, type: typeof(GenericErrorModel), description: "Unexpected error")]
        public virtual async Task<IActionResult> UnfollowUserByUsername([FromRoute (Name = "username")][Required]string username)
        {
            var unfollowUserResponse = await Mediator.Send(new UnfollowUserCommand { Username = username });
            
            if (unfollowUserResponse.Result != OperationResult.Success)
                return UnsuccessfulResponseResult(unfollowUserResponse);

            return Ok(unfollowUserResponse.Response.UnfollowedProfile.ToProfileResponse());
        }
    }
}
