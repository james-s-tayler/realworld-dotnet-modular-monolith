using System;

namespace Conduit.Identity.Domain.Contracts.LoginUser
{
    public class LoginUserResult
    {
        public LoggedInUserDTO LoggedInUser { get; }

        private LoginUserResult() {}

        private LoginUserResult(LoggedInUserDTO loggedInUser)
        {
            LoggedInUser = loggedInUser ?? throw new ArgumentNullException(nameof(loggedInUser));
        }

        public bool IsAuthenticated => LoggedInUser != null;

        public static LoginUserResult FailedLoginResult()
        {
            return new LoginUserResult();
        }

        public static LoginUserResult SuccessfulLoginResult(LoggedInUserDTO loggedInUserDto)
        {
            return new LoginUserResult(loggedInUserDto);
        }
    }
}