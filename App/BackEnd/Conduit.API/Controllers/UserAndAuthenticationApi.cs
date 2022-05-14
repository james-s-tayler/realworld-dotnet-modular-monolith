/*
 * Conduit API
 *
 * Conduit API
 *
 * The version of the OpenAPI document: 1.0.0
 * 
 * Generated by: https://openapi-generator.tech
 */

using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Conduit.API.Attributes;
using Conduit.API.Models;
using Conduit.Core.PipelineBehaviors;
using Conduit.Identity.Domain.Contracts.Commands.LoginUser;
using Conduit.Identity.Domain.Contracts.Commands.RegisterUser;
using Conduit.Identity.Domain.Contracts.Commands.UpdateUser;
using Conduit.Identity.Domain.Contracts.Queries.GetCurrentUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Conduit.API.Controllers
{ 
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
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
        /// <param name="request">Details of the new user to register</param>
        /// <response code="201">OK</response>
        /// <response code="422">Unexpected error</response>
        [HttpPost]
        [AllowAnonymous]
        [Route("/api/users")]
        [ValidateModelState]
        [SwaggerOperation("CreateUser")]
        [SwaggerResponse(statusCode: 201, type: typeof(UserResponse), description: "OK")]
        [SwaggerResponse(statusCode: 422, type: typeof(GenericErrorModel), description: "Unexpected error")]
        public virtual async Task<IActionResult> CreateUser([FromBody]NewUserRequest request)
        {
            var registerUserResponse = await _mediator.Send(new RegisterUserCommand
            {
                NewUser = new NewUserDTO
                {
                    Email = request.User.Email,
                    Username = request.User.Username,
                    Password = request.User.Password
                }
            });
            
            if (registerUserResponse.Result != OperationResult.Success)
                return UnsuccessfulResponseResult(registerUserResponse);

            _logger.LogInformation($"Registered {request.User.Username} - UserId:{registerUserResponse.Response.UserId}");

            var loginResponse = await _mediator.Send(new LoginUserCommand
            {
                UserCredentials = new UserCredentialsDTO
                {
                    Email = request.User.Email,
                    Password = request.User.Password
                }
            });
            
            if (loginResponse.Result != OperationResult.Success)
                return UnsuccessfulResponseResult(registerUserResponse);
            if (!loginResponse.Response.IsAuthenticated)
                throw new ApplicationException("Failed to authenticated newly created user.");

            var loggedInUser = loginResponse.Response.LoggedInUser;
            
            //hmm this was generated by swagger, but it's just another layer of mapping - would like to get rid of it
            var newUser = new UserResponse
            {
                User = new User
                {
                    Email = loggedInUser.Email,
                    Username = loggedInUser.Username,
                    Token = loggedInUser.Token,
                    Bio = loggedInUser.Bio,
                    Image = loggedInUser.Image
                }
            };
            
            return StatusCode((int) HttpStatusCode.Created, newUser);
        }

        private ObjectResult UnsuccessfulResponseResult<T>(OperationResponse<T> operationResponse) where T : class
        {
            if (operationResponse.Result == OperationResult.Success)
                throw new InvalidOperationException("OperationResult was Success");

            var errors = new GenericErrorModel
            {
                Errors = new GenericErrorModelErrors
                {
                    Body = operationResponse.ErrorMessages
                }
            };
            switch (operationResponse.Result)
            {
                case OperationResult.NotAuthenticated:
                    return StatusCode((int)HttpStatusCode.Unauthorized, errors);
                case OperationResult.NotAuthorized:
                    return StatusCode((int)HttpStatusCode.Forbidden, errors);
                case OperationResult.ValidationError:
                    return StatusCode((int)HttpStatusCode.UnprocessableEntity, errors);
                default:
                    return StatusCode((int)HttpStatusCode.InternalServerError, errors);
            }
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
        [ValidateModelState]
        [SwaggerOperation("GetCurrentUser")]
        [SwaggerResponse(statusCode: 200, type: typeof(UserResponse), description: "OK")]
        [SwaggerResponse(statusCode: 422, type: typeof(GenericErrorModel), description: "Unexpected error")]
        public virtual async Task<IActionResult> GetCurrentUser()
        {
            var getCurrentUserResponse = await _mediator.Send(new GetCurrentUserQuery());
            
            if (getCurrentUserResponse.Result != OperationResult.Success)
                return UnsuccessfulResponseResult(getCurrentUserResponse);
            
            var currentUser = getCurrentUserResponse.Response.CurrentUser;

            var user = new UserResponse
            {
                User = new User
                {
                    Email = currentUser.Email,
                    Username = currentUser.Username,
                    Token = currentUser.Token,
                    Bio = currentUser.Bio,
                    Image = currentUser.Image
                }
            };
            
            return Ok(user);
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
        [AllowAnonymous]
        [Route("/api/users/login")]
        [Consumes("application/json")]
        [ValidateModelState]
        [SwaggerOperation("Login")]
        [SwaggerResponse(statusCode: 200, type: typeof(UserResponse), description: "OK")]
        [SwaggerResponse(statusCode: 422, type: typeof(GenericErrorModel), description: "Unexpected error")]
        public virtual async Task<IActionResult> Login([FromBody]LoginUserRequest request)
        {
            //this can purely be auth then once authed simply redirect to GetCurrentUser!
            
            var loginResponse = await _mediator.Send(new LoginUserCommand
            {
                UserCredentials = new UserCredentialsDTO
                {
                    Email = request.User.Email,
                    Password = request.User.Password
                }
            });
            
            if (loginResponse.Result != OperationResult.Success)
                return UnsuccessfulResponseResult(loginResponse);
            if (!loginResponse.Response.IsAuthenticated)
                return Unauthorized();
            
            //need to either redirect to GetCurrentUser or implement that query and call it here
            var loggedInUser = loginResponse.Response.LoggedInUser;
            var user = new UserResponse
            {
                User = new User
                {
                    Email = loggedInUser.Email,
                    Username = loggedInUser.Username,
                    Token = loggedInUser.Token,
                    Bio = loggedInUser.Bio,
                    Image = loggedInUser.Image
                }
            };
            return Ok(user);
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
        [Consumes("application/json")]
        [ValidateModelState]
        [SwaggerOperation("UpdateCurrentUser")]
        [SwaggerResponse(statusCode: 200, type: typeof(UserResponse), description: "OK")]
        [SwaggerResponse(statusCode: 422, type: typeof(GenericErrorModel), description: "Unexpected error")]
        public virtual async Task<IActionResult> UpdateCurrentUser([FromBody]UpdateUserRequest body)
        {
            var getCurrentUserResponse = await _mediator.Send(new UpdateUserCommand
            {
                UpdateUser = new UpdateUserDTO
                {
                    Email = body.User.Email,
                    Bio = body.User.Bio,
                    Image = body.User.Image,
                    Username = body.User.Username
                }
            });
            
            if (getCurrentUserResponse.Result != OperationResult.Success)
                return UnsuccessfulResponseResult(getCurrentUserResponse);
            
            var updatedUser = getCurrentUserResponse.Response.UpdatedUser;

            var user = new UserResponse
            {
                User = new User
                {
                    Email = updatedUser.Email,
                    Username = updatedUser.Username,
                    Token = updatedUser.Token,
                    Bio = updatedUser.Bio,
                    Image = updatedUser.Image
                }
            };
            
            return Ok(user);
        }
    }
}
