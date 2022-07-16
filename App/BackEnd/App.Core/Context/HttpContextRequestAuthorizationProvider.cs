using Microsoft.AspNetCore.Http;

namespace App.Core.Context
{
    public class HttpContextRequestAuthorizationProvider : IRequestAuthorizationProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextRequestAuthorizationProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetRequestAuthorization()
        {
            var authHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"][0];
            return authHeader.Replace("Bearer ", "");
        }
    }
}