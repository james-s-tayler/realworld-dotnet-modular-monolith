/*
 * Conduit API
 *
 * Conduit API
 *
 * The version of the OpenAPI document: 1.0.0
 * 
 * Generated by: https://openapi-generator.tech
 */

using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Newtonsoft.Json;
using Conduit.API.Attributes;
using Conduit.API.Models;
using Conduit.Identity.Domain.Contracts.RegisterUser;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Conduit.API.Controllers
{ 
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class UserAndAuthenticationApiController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UserAndAuthenticationApiController> _logger;

        public UserAndAuthenticationApiController(IMediator mediator, 
            ILogger<UserAndAuthenticationApiController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        /// <remarks>Register a new user</remarks>
        /// <param name="body">Details of the new user to register</param>
        /// <response code="201">OK</response>
        /// <response code="422">Unexpected error</response>
        [HttpPost]
        [Route("/api/users")]
        [Consumes("application/json")]
        [ValidateModelState]
        [SwaggerOperation("CreateUser")]
        [SwaggerResponse(statusCode: 201, type: typeof(UserResponse), description: "OK")]
        [SwaggerResponse(statusCode: 422, type: typeof(GenericErrorModel), description: "Unexpected error")]
        public virtual async Task<IActionResult> CreateUser([FromBody]NewUserRequest body)
        {
            var result = await _mediator.Send(new RegisterUserCommand
            {
                NewUser = new NewUserDTO
                {
                    Email = body.User.Email,
                    Username = body.User.Username,
                    Password = body.User.Password
                }
            });

            _logger.LogInformation($"Registered {body.User.Username} - UserId:{result.Response.UserId}");

            var newUser = new UserResponse
            {
                User = new User
                {
                    Email = body.User.Email,
                    Username = body.User.Username,
                    Bio = "I work at statefarm",
                    Image = null,
                    Token = "yolo"
                }
            };
            
            return StatusCode((int) HttpStatusCode.Created, newUser);
        }

        /// <summary>
        /// Get current user
        /// </summary>
        /// <remarks>Gets the currently logged-in user</remarks>
        /// <response code="200">OK</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="422">Unexpected error</response>
        [HttpGet]
        [Route("/api/user")]
        [Authorize(Policy = "Token")]
        [ValidateModelState]
        [SwaggerOperation("GetCurrentUser")]
        [SwaggerResponse(statusCode: 200, type: typeof(UserResponse), description: "OK")]
        [SwaggerResponse(statusCode: 422, type: typeof(GenericErrorModel), description: "Unexpected error")]
        public virtual async Task<IActionResult> GetCurrentUser()
        {
            //implement a UserContext object
            
            
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200, default(UserResponse));
            //TODO: Uncomment the next line to return response 401 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(401);
            //TODO: Uncomment the next line to return response 422 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(422, default(GenericErrorModel));
            string exampleJson = null;
            exampleJson = "{\n  \"user\" : {\n    \"image\" : \"image\",\n    \"bio\" : \"bio\",\n    \"email\" : \"email\",\n    \"token\" : \"token\",\n    \"username\" : \"username\"\n  }\n}";
            
            var example = exampleJson != null
            ? JsonConvert.DeserializeObject<UserResponse>(exampleJson)
            : default(UserResponse);
            //TODO: Change the data returned
            return new ObjectResult(example);
        }

        /// <summary>
        /// Existing user login
        /// </summary>
        /// <remarks>Login for existing user</remarks>
        /// <param name="body">Credentials to use</param>
        /// <response code="200">OK</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="422">Unexpected error</response>
        [HttpPost]
        [Route("/api/users/login")]
        [Consumes("application/json")]
        [ValidateModelState]
        [SwaggerOperation("Login")]
        [SwaggerResponse(statusCode: 200, type: typeof(UserResponse), description: "OK")]
        [SwaggerResponse(statusCode: 422, type: typeof(GenericErrorModel), description: "Unexpected error")]
        public virtual async Task<IActionResult> Login([FromBody]LoginUserRequest body)
        {
            
            //this can purely be auth then once authed simply redirect to GetCurrentUser!
            
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200, default(UserResponse));
            //TODO: Uncomment the next line to return response 401 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(401);
            //TODO: Uncomment the next line to return response 422 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(422, default(GenericErrorModel));
            string exampleJson = null;
            exampleJson = "{\n  \"user\" : {\n    \"image\" : \"image\",\n    \"bio\" : \"bio\",\n    \"email\" : \"email\",\n    \"token\" : \"token\",\n    \"username\" : \"username\"\n  }\n}";
            
            var example = exampleJson != null
            ? JsonConvert.DeserializeObject<UserResponse>(exampleJson)
            : default(UserResponse);
            //TODO: Change the data returned
            return new ObjectResult(example);
        }

        /// <summary>
        /// Update current user
        /// </summary>
        /// <remarks>Updated user information for current user</remarks>
        /// <param name="body">User details to update. At least **one** field is required.</param>
        /// <response code="200">OK</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="422">Unexpected error</response>
        [HttpPut]
        [Route("/api/user")]
        [Authorize(Policy = "Token")]
        [Consumes("application/json")]
        [ValidateModelState]
        [SwaggerOperation("UpdateCurrentUser")]
        [SwaggerResponse(statusCode: 200, type: typeof(UserResponse), description: "OK")]
        [SwaggerResponse(statusCode: 422, type: typeof(GenericErrorModel), description: "Unexpected error")]
        public virtual async Task<IActionResult> UpdateCurrentUser([FromBody]UpdateUserRequest body)
        {

            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200, default(UserResponse));
            //TODO: Uncomment the next line to return response 401 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(401);
            //TODO: Uncomment the next line to return response 422 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(422, default(GenericErrorModel));
            string exampleJson = null;
            exampleJson = "{\n  \"user\" : {\n    \"image\" : \"image\",\n    \"bio\" : \"bio\",\n    \"email\" : \"email\",\n    \"token\" : \"token\",\n    \"username\" : \"username\"\n  }\n}";
            
            var example = exampleJson != null
            ? JsonConvert.DeserializeObject<UserResponse>(exampleJson)
            : default(UserResponse);
            //TODO: Change the data returned
            return new ObjectResult(example);
        }
    }
}
