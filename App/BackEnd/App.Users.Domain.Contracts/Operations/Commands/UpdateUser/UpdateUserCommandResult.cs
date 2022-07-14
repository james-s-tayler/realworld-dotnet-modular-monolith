using App.Core.DataAccess;
using App.Users.Domain.Contracts.DTOs;
using MediatR;

namespace App.Users.Domain.Contracts.Operations.Commands.UpdateUser
{
    public class UpdateUserCommandResult : ContractModel, INotification
    {
        public UserDTO UpdatedUser { get; set; }
    }
}