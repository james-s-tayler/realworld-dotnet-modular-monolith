using System.Security.Claims;

namespace App.Core.Context
{
    public interface IRequestClaimsPrincipalProvider
    {
        ClaimsPrincipal GetClaimsPrincipal();
        void SetClaimsPrincipal(ClaimsPrincipal principal);
    }
}