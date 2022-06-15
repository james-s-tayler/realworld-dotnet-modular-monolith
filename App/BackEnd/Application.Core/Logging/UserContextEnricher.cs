using Application.Core.Context;
using JetBrains.Annotations;
using Serilog.Core;
using Serilog.Events;
using TracerAttributes;

namespace Application.Core.Logging
{
    [NoTrace]
    public class UserContextEnricher : ILogEventEnricher
    {
        private readonly IUserContext _userContext;

        public UserContextEnricher(IUserContext userContext)
        {
            _userContext = userContext;
        }
        
        public void Enrich([NotNull] LogEvent logEvent, [NotNull] ILogEventPropertyFactory propertyFactory)
        {
            if (_userContext.IsAuthenticated)
            {
                LogEventProperty userId = propertyFactory.CreateProperty("UserId", _userContext.UserId);
                logEvent.AddPropertyIfAbsent(userId);
            }
        }
    }
}