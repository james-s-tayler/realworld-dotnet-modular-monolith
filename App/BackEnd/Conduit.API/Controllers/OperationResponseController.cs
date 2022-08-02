using System;
using System.Net;
using App.Core.PipelineBehaviors.OperationResponse;
using Conduit.API.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class OperationResponseController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        protected IMediator Mediator { get; }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        public OperationResponseController(IMediator mediator)
        {
            Mediator = mediator;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operationResponse"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        protected ObjectResult UnsuccessfulResponseResult<T>(OperationResponse<T> operationResponse) where T : class
        {
            if (operationResponse.Result == OperationResult.Success)
                throw new InvalidOperationException("OperationResult was Success");

            var errors = new GenericErrorModel
            {
                Errors = new GenericErrorModelErrors
                {
                    Body = operationResponse.Errors
                }
            };
            switch (operationResponse.Result)
            {
                case OperationResult.NotFound:
                    return StatusCode(StatusCodes.Status404NotFound, errors);
                case OperationResult.ValidationError:
                case OperationResult.InvalidRequest:
                    return StatusCode(StatusCodes.Status422UnprocessableEntity, errors);
                case OperationResult.NotAuthenticated:
                    return StatusCode(StatusCodes.Status401Unauthorized, errors);
                case OperationResult.NotAuthorized:
                    return StatusCode(StatusCodes.Status403Forbidden, errors);
                case OperationResult.NotImplemented:
                    return StatusCode(StatusCodes.Status501NotImplemented, errors);
                case OperationResult.UnhandledException:
                default:
                    return StatusCode(StatusCodes.Status500InternalServerError, errors);
            }
        }
    }
}