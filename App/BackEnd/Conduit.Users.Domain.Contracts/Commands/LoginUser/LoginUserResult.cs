using System;

namespace Conduit.Identity.Domain.Contracts.Commands.LoginUser
{
    public class LoginUserResult
    {
        public UserDTO LoggedInUser { get; }

        private LoginUserResult() {}

        private LoginUserResult(UserDTO user)
        {
            LoggedInUser = user ?? throw new ArgumentNullException(nameof(user));
        }

        public bool IsAuthenticated => LoggedInUser != null;

        public static LoginUserResult FailedLoginResult()
        {
            return new LoginUserResult();
        }

        public static LoginUserResult SuccessfulLoginResult(UserDTO userDto)
        {
            return new LoginUserResult(userDto);
        }
    }
}