using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace App.Core.Context
{
    public class HttpContextRequestClaimsPrincipalProvider : IRequestClaimsPrincipalProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextRequestClaimsPrincipalProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public ClaimsPrincipal GetClaimsPrincipal()
        {
            return _httpContextAccessor.HttpContext?.User ?? new ClaimsPrincipal();
        }

        public void SetClaimsPrincipal(ClaimsPrincipal principal)
        {
            _httpContextAccessor.HttpContext!.User = principal;
        }
    }
}