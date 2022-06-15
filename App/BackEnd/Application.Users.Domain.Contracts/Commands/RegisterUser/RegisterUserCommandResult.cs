using Application.Core.DataAccess;
using MediatR;

namespace Application.Users.Domain.Contracts.Commands.RegisterUser
{
    public class RegisterUserCommandResult : ContractModel, INotification
    {
        public UserDTO RegisteredUser { get; set; }
    }
}