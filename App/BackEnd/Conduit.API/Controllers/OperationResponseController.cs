using System;
using System.Net;
using Conduit.API.Models;
using Conduit.Core.PipelineBehaviors.OperationResponse;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.API.Controllers
{
    public class OperationResponseController : ControllerBase
    {
        protected IMediator Mediator { get; }
        
        public OperationResponseController(IMediator mediator)
        {
            Mediator = mediator;
        }

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
                case OperationResult.NotAuthenticated:
                    return StatusCode((int)HttpStatusCode.Unauthorized, errors);
                case OperationResult.NotAuthorized:
                    return StatusCode((int)HttpStatusCode.Forbidden, errors);
                case OperationResult.ValidationError:
                    return StatusCode((int)HttpStatusCode.UnprocessableEntity, errors);
                case OperationResult.NotImplemented:
                    return StatusCode((int)HttpStatusCode.NotImplemented, errors);
                case OperationResult.UnhandledException:
                default:
                    return StatusCode((int)HttpStatusCode.InternalServerError, errors);
            }
        }
    }
}