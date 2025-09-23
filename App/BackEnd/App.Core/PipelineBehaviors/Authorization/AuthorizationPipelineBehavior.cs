using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using App.Core.Context;
using App.Core.PipelineBehaviors.OperationResponse;
using MediatR;

namespace App.Core.PipelineBehaviors.Authorization
{
    //adapted from: https://medium.com/@austin.davies0101/creating-a-basic-authorization-pipeline-with-mediatr-and-asp-net-core-c257fe3cc76b
    //              https://levelup.gitconnected.com/handling-authorization-in-clean-architecture-with-asp-net-core-and-mediatr-6b91eeaa4d15
    public class AuthorizationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : class
    {

        private readonly IUserContext _userContext;
        private readonly IEnumerable<IAuthorizer<TRequest>> _authorizers;

        public AuthorizationPipelineBehavior(IEnumerable<IAuthorizer<TRequest>> authorizers,
            IUserContext userContext)
        {
            _authorizers = authorizers;
            _userContext = userContext;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if ( !IsOperationResponse() )
                throw new InvalidOperationException("Domain operations must be of type OperationResponse<T>");

            if ( !_userContext.IsAuthenticated )
            {
                if ( IsAllowUnauthenticated() )
                    return await next();

                return OperationResponseFactory.NotAuthenticated<TRequest, TResponse>();
            }

            foreach ( var authorizer in _authorizers )
            {
                var result = await authorizer.AuthorizeAsync(request, cancellationToken);
                if ( !result.IsAuthorized )
                {
                    return OperationResponseFactory.NotAuthorized<TRequest, TResponse>(result.FailureMessage);
                }
            }

            return await next();
        }

        private bool IsOperationResponse()
        {
            var responseType = typeof(TResponse);
            return responseType.IsGenericType && responseType.Name.Contains("OperationResponse");
        }

        private bool IsAllowUnauthenticated()
        {
            var requestType = typeof(TRequest);
            return requestType.CustomAttributes.Any(attribute => attribute.AttributeType == typeof(AllowUnauthenticatedAttribute));
        }
    }
}