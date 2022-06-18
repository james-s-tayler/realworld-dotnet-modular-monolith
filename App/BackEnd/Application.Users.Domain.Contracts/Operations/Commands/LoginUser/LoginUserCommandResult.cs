using Application.Core.DataAccess;
using Application.Users.Domain.Contracts.DTOs;
using JetBrains.Annotations;
using MediatR;

namespace Application.Users.Domain.Contracts.Operations.Commands.LoginUser
{
    public class LoginUserCommandResult : ContractModel, INotification
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