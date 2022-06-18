using Application.Core.DataAccess;
using Application.Users.Domain.Contracts.DTOs;
using MediatR;

namespace Application.Users.Domain.Contracts.Operations.Commands.RegisterUser
{
    public class RegisterUserCommandResult : ContractModel, INotification
    {
        public UserDTO RegisteredUser { get; set; }
    }
}