using Application.Core.DataAccess;
using JetBrains.Annotations;

namespace Application.Users.Domain.Contracts.Commands.LoginUser
{
    public class LoginUserCommandResult : ContractModel
    {
        public UserDTO LoggedInUser { get; }

        private LoginUserCommandResult() {}

        private LoginUserCommandResult([NotNull] UserDTO user)
        {
            LoggedInUser = user;
        }

        public bool IsAuthenticated => LoggedInUser != null;

        public static LoginUserCommandResult FailedLoginResult()
        {
            return new LoginUserCommandResult();
        }

        public static LoginUserCommandResult SuccessfulLoginResult(UserDTO userDto)
        {
            return new LoginUserCommandResult(userDto);
        }
    }
}