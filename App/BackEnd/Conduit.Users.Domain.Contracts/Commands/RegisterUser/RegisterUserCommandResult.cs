using Conduit.Core.DataAccess;
using MediatR;

namespace Conduit.Users.Domain.Contracts.Commands.RegisterUser
{
    public class RegisterUserCommandResult : ContractModel, INotification
    {
        public UserDTO RegisteredUser { get; set; }
    }
}