using System.Diagnostics.CodeAnalysis;
using App.Core.DataAccess;
using App.Users.Domain.Contracts.DTOs;
using MediatR;

namespace App.Users.Domain.Contracts.Operations.Commands.LoginUser
{
    [ExcludeFromCodeCoverage]
    public class LoginUserCommandResult : ContractModel, INotification
    {
        public UserDTO LoggedInUser { get; }

        private LoginUserCommandResult() { }

        private LoginUserCommandResult([JetBrains.Annotations.NotNull] UserDTO user)
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