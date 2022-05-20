using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Conduit.Core.Logging
{
    //this looks a bit odd but we're subclassing this so that it's the right shape for the framework
    //yet we're actually using the decorator pattern to simply delegate to an instance of JwtBearerEvents
    public class JwtBearerEventsLogger : JwtBearerEvents
    {

        private readonly JwtBearerEvents _jwtBearerEvents;

        public JwtBearerEventsLogger()
        {
            _jwtBearerEvents = new JwtBearerEvents();
        }
        
        public JwtBearerEventsLogger(JwtBearerEvents jwtBearerEvents)
        {
            _jwtBearerEvents = jwtBearerEvents;
        }

        /// <summary>
        /// Invoked if exceptions are thrown during request processing. The exceptions will be re-thrown after this event unless suppressed.
        /// </summary>
        public override Task AuthenticationFailed(AuthenticationFailedContext context)
        {
            var logger = context.HttpContext.RequestServices.GetService<ILoggerFactory>()!.CreateLogger<JwtBearerEventsLogger>();
            logger.LogTrace(context.Exception, "Authentication failed for scheme: {Scheme}", context.Scheme.Name);
            return _jwtBearerEvents.AuthenticationFailed(context);
        }

        /// <summary>
        /// Invoked if Authorization fails and results in a Forbidden response  
        /// </summary>
        public override  Task Forbidden(ForbiddenContext context)
        {
            var logger = context.HttpContext.RequestServices.GetService<ILoggerFactory>()!.CreateLogger<JwtBearerEventsLogger>();
            logger.LogTrace("Auth forbidden for scheme: {Scheme}", context.Scheme.Name);
            return _jwtBearerEvents.Forbidden(context);
        }

        /// <summary>
        /// Invoked when a protocol message is first received.
        /// </summary>
        public override Task MessageReceived(MessageReceivedContext context)
        {
            var logger = context.HttpContext.RequestServices.GetService<ILoggerFactory>()!.CreateLogger<JwtBearerEventsLogger>();
            
            logger.LogTrace("Message received for scheme: {Scheme}, Token: {Token}, JwtBearerOptions: {@JwtBearerOptions}, AuthenticationResult: {@AuthenticationResult}", 
                context.Scheme.Name,
                context.Token,
                context.Options,
                context.Result);

            return _jwtBearerEvents.MessageReceived(context);
        }

        /// <summary>
        /// Invoked after the security token has passed validation and a ClaimsIdentity has been generated.
        /// </summary>
        public override Task TokenValidated(TokenValidatedContext context)
        {
            var logger = context.HttpContext.RequestServices.GetService<ILoggerFactory>()!.CreateLogger<JwtBearerEventsLogger>();
            logger.LogTrace("Token validated for scheme: {Scheme}", context.Scheme.Name);
            return _jwtBearerEvents.TokenValidated(context);
        }

        /// <summary>
        /// Invoked before a challenge is sent back to the caller.
        /// </summary>
        public override Task Challenge(JwtBearerChallengeContext context)
        {
            var logger = context.HttpContext.RequestServices.GetService<ILoggerFactory>()!.CreateLogger<JwtBearerEventsLogger>();
            logger.LogTrace(context.AuthenticateFailure, "Challenged issued for scheme: {Scheme}", context.Scheme.Name);
            return _jwtBearerEvents.Challenge(context);
        }
    }
}