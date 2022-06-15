using Microsoft.AspNetCore.Http;
using TracerAttributes;

namespace Application.Core.Context
{
    [NoTrace]
    public class ApiContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool IsAuthenticated => _httpContextAccessor.HttpContext.User.Identity is {IsAuthenticated: true};

        public int UserId => int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("user_id")!.Value);
        public string Username => _httpContextAccessor.HttpContext.User.FindFirst("username")!.Value;
        public string Email => _httpContextAccessor.HttpContext.User.FindFirst("email")!.Value;

        public string Token
        {
            get
            {
                var authHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"][0];
                return authHeader.Replace("Bearer ", "");
            }
        }
    }
}