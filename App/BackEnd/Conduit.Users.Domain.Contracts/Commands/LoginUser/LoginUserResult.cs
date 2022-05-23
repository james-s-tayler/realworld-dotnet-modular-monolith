using Conduit.Core.DataAccess;
using JetBrains.Annotations;

namespace Conduit.Users.Domain.Contracts.Commands.LoginUser
{
    public class LoginUserResult : ContractModel
    {
        public UserDTO LoggedInUser { get; }

        private LoginUserResult() {}

        private LoginUserResult([NotNull] UserDTO user)
        {
            LoggedInUser = user;
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