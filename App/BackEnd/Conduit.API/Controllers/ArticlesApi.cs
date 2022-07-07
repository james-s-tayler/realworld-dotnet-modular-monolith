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
using Application.Content.Domain.Contracts.Operations.Commands.DeleteArticle;
using Application.Content.Domain.Contracts.Operations.Commands.EditArticle;
using Application.Content.Domain.Contracts.Operations.Commands.PublishArticle;
using Application.Content.Domain.Contracts.Operations.Queries.GetSingleArticle;
using Application.Content.Domain.Contracts.Operations.Queries.ListArticles;
using Application.Core.PipelineBehaviors.OperationResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Conduit.API.Attributes;
using Conduit.API.Models;
using Conduit.API.Models.Mappers;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Conduit.API.Controllers
{ 
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    [SwaggerResponse(statusCode: 422, type: typeof(GenericErrorModel), description: "Unexpected error")]
    public class ArticlesApiController : OperationResponseController
    {
        public ArticlesApiController(IMediator mediator) : base(mediator)
        {
        }
        
        /// <summary>
        /// Create an article
        /// </summary>
        /// <remarks>Create an article. Auth is required</remarks>
        /// <param name="request">Article to create</param>
        /// <response code="201">OK</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="422">Unexpected error</response>
        [HttpPost]
        [Route("/api/articles")]
        [ValidateModelState]
        [SwaggerOperation("CreateArticle")]
        [ProducesResponseType(statusCode: 201, type: typeof(SingleArticleResponse))]
        public virtual async Task<IActionResult> CreateArticle([FromBody]NewArticleRequest request)
        {
            var publishArticleResponse = await Mediator.Send(new PublishArticleCommand { NewArticle = request.Article.ToPublishArticleDto() });
            
            if (publishArticleResponse.Result != OperationResult.Success)
                return UnsuccessfulResponseResult(publishArticleResponse);

            return StatusCode(StatusCodes.Status201Created, publishArticleResponse.Response.Article.ToSingleArticleResponse());
        }

        /// <summary>
        /// Delete an article
        /// </summary>
        /// <remarks>Delete an article. Auth is required</remarks>
        /// <param name="slug">Slug of the article to delete</param>
        /// <response code="200">OK</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="422">Unexpected error</response>
        [HttpDelete]
        [Route("/api/articles/{slug}")]
        [ValidateModelState]
        [SwaggerOperation("DeleteArticle")]
        public virtual async Task<IActionResult> DeleteArticle([FromRoute (Name = "slug")][Required]string slug)
        {
            var deleteArticleResponse = await Mediator.Send(new DeleteArticleCommand { Slug = slug });
            
            if (deleteArticleResponse.Result != OperationResult.Success)
                return UnsuccessfulResponseResult(deleteArticleResponse);

            return Ok();
        }

        /// <summary>
        /// Get an article
        /// </summary>
        /// <remarks>Get an article. Auth not required</remarks>
        /// <param name="slug">Slug of the article to get</param>
        /// <response code="200">OK</response>
        /// <response code="422">Unexpected error</response>
        [HttpGet]
        [AllowAnonymous]
        [Route("/api/articles/{slug}")]
        [ValidateModelState]
        [SwaggerOperation("GetArticle")]
        [SwaggerResponse(statusCode: 200, type: typeof(SingleArticleResponse), description: "OK")]
        public virtual async Task<IActionResult> GetArticle([FromRoute (Name = "slug")][Required]string slug)
        {
            var followUserResponse = await Mediator.Send(new GetSingleArticleQuery { Slug = slug });
            
            if (followUserResponse.Result != OperationResult.Success)
                return UnsuccessfulResponseResult(followUserResponse);

            return Ok(followUserResponse.Response.Article.ToSingleArticleResponse());
        }

        /// <summary>
        /// Get recent articles globally
        /// </summary>
        /// <remarks>Get most recent articles globally. Use query parameters to filter results. Auth is optional</remarks>
        /// <param name="tag">Filter by tag</param>
        /// <param name="author">Filter by author (username)</param>
        /// <param name="favorited">Filter by favorites of a user (username)</param>
        /// <param name="limit">Limit number of articles returned (default is 20)</param>
        /// <param name="offset">Offset/skip number of articles (default is 0)</param>
        /// <response code="200">OK</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="422">Unexpected error</response>
        [HttpGet]
        [AllowAnonymous]
        [Route("/api/articles")]
        [ValidateModelState]
        [SwaggerOperation("GetArticles")]
        [SwaggerResponse(statusCode: 200, type: typeof(MultipleArticlesResponse), description: "OK")]
        public virtual async Task<IActionResult> GetArticles([FromQuery (Name = "tag")]string tag, [FromQuery (Name = "author")]string author, [FromQuery (Name = "favorited")]string favorited, [FromQuery (Name = "limit")]int? limit, [FromQuery (Name = "offset")]int? offset)
        {
            var listArticlesQuery = new ListArticlesQuery
            {
                Tag = tag,
                AuthorUsername = author,
                FavoritedByUsername = favorited
            };
            if (limit.HasValue)
            {
                listArticlesQuery.Limit = limit.Value;
            }

            if (offset.HasValue)
            {
                listArticlesQuery.Offset = offset.Value;
            }
            
            var listArticlesResponse = await Mediator.Send(listArticlesQuery);
            
            if (listArticlesResponse.Result != OperationResult.Success)
                return UnsuccessfulResponseResult(listArticlesResponse);

            return Ok(listArticlesResponse.Response.Articles.ToMultipleArticlesResponse());
        }

        /// <summary>
        /// Get recent articles from users you follow
        /// </summary>
        /// <remarks>Get most recent articles from users you follow. Use query parameters to limit. Auth is required</remarks>
        /// <param name="limit">Limit number of articles returned (default is 20)</param>
        /// <param name="offset">Offset/skip number of articles (default is 0)</param>
        /// <response code="200">OK</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="422">Unexpected error</response>
        [HttpGet]
        [Route("/api/articles/feed")]
        [ValidateModelState]
        [SwaggerOperation("GetArticlesFeed")]
        [SwaggerResponse(statusCode: 200, type: typeof(MultipleArticlesResponse), description: "OK")]
        public virtual async Task<IActionResult> GetArticlesFeed([FromQuery (Name = "limit")]int? limit, [FromQuery (Name = "offset")]int? offset)
        {
            return StatusCode((int)HttpStatusCode.NotImplemented);
        }

        /// <summary>
        /// Update an article
        /// </summary>
        /// <remarks>Update an article. Auth is required</remarks>
        /// <param name="slug">Slug of the article to update</param>
        /// <param name="article">Article to update</param>
        /// <response code="200">OK</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="422">Unexpected error</response>
        [HttpPut]
        [Route("/api/articles/{slug}")]
        [Consumes("application/json")]
        [ValidateModelState]
        [SwaggerOperation("UpdateArticle")]
        [SwaggerResponse(statusCode: 200, type: typeof(SingleArticleResponse), description: "OK")]
        public virtual async Task<IActionResult> UpdateArticle([FromRoute (Name = "slug")][Required]string slug, [FromBody]UpdateArticleRequest article)
        {
            var editArticleResponse = await Mediator.Send(new EditArticleCommand
            {
                Slug = slug,
                UpdatedArticle = article.ToEditArticleDto()
            });
            
            if (editArticleResponse.Result != OperationResult.Success)
                return UnsuccessfulResponseResult(editArticleResponse);

            return Ok(editArticleResponse.Response.Article.ToSingleArticleResponse());
        }
    }
}
