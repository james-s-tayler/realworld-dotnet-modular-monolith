using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Net.Http.Headers;

namespace Application.Core.Logging
{
    //JwtBearerEvents hardcodes the scheme as "Bearer" - this class allows using a custom scheme name
    public class CustomSchemeJwtBearerEvents : JwtBearerEvents
    {
        private readonly string _scheme;

        public CustomSchemeJwtBearerEvents([NotNull] string scheme)
        {
            _scheme = scheme;
        }

        /// <summary>
        /// Invoked when a protocol message is first received.
        /// </summary>
        public override Task MessageReceived(MessageReceivedContext context)
        {
            string authorization = context.Request.Headers[HeaderNames.Authorization];
            string token = null;

            // If no authorization header found, nothing to process further
            if (string.IsNullOrEmpty(authorization))
            {
                context.NoResult();
                return OnMessageReceived(context);
            }

            if (authorization.StartsWith($"{_scheme} ", StringComparison.OrdinalIgnoreCase))
            {
                token = authorization.Substring($"{_scheme} ".Length).Trim();
            }

            // If no token found, no further work possible
            if (string.IsNullOrEmpty(token))
            {
                context.NoResult();
                return OnMessageReceived(context);
            }

            context.Token = token;
            return OnMessageReceived(context);
        }
    }
}