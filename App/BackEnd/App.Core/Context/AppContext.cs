using JetBrains.Annotations;
using TracerAttributes;

namespace App.Core.Context
{
    [NoTrace]
    public class AppContext : IUserContext
    {
        private readonly IRequestClaimsPrincipalProvider _requestClaimsPrincipalProvider;
        private readonly IRequestAuthorizationProvider _requestAuthorizationProvider;

        public AppContext([NotNull] IRequestClaimsPrincipalProvider requestClaimsPrincipalProvider,
            [NotNull] IRequestAuthorizationProvider requestAuthorizationProvider)
        {
            _requestClaimsPrincipalProvider = requestClaimsPrincipalProvider;
            _requestAuthorizationProvider = requestAuthorizationProvider;
        }

        public bool IsAuthenticated => _requestClaimsPrincipalProvider.GetClaimsPrincipal().Identity is { IsAuthenticated: true };

        public int UserId => int.Parse(_requestClaimsPrincipalProvider.GetClaimsPrincipal().FindFirst("user_id")!.Value);
        public string Username => _requestClaimsPrincipalProvider.GetClaimsPrincipal().FindFirst("username")!.Value;
        public string Email => _requestClaimsPrincipalProvider.GetClaimsPrincipal().FindFirst("email")!.Value;

        public string Token => _requestAuthorizationProvider.GetRequestAuthorization();
    }
}