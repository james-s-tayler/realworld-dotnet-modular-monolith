using System;
using System.Text;
using Conduit.Core.Context;
using Serilog.Core;
using Serilog.Events;

namespace Conduit.Core.Logging
{
    public class UserContextEnricher : ILogEventEnricher
    {
        private readonly IUserContext _userContext;

        public UserContextEnricher(IUserContext userContext)
        {
            _userContext = userContext;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (logEvent is null)
                throw new ArgumentNullException(nameof(logEvent));

            if (propertyFactory is null)
                throw new ArgumentNullException(nameof(propertyFactory));

            if (_userContext.IsAuthenticated)
            {
                LogEventProperty userId = propertyFactory.CreateProperty("UserId", _userContext.UserId);
                logEvent.AddPropertyIfAbsent(userId);
            }
        }
    }
}