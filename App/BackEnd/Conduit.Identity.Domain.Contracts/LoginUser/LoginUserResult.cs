using System;

namespace Conduit.Identity.Domain.Contracts.LoginUser
{
    public class LoginUserResult
    {
        public string Token { get; } = "";

        public LoginUserResult() {}

        public LoginUserResult(string token)
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentNullException(token);

            Token = token;
        }

        public bool IsAuthenticated => !Token.Equals(string.Empty);
    }
}