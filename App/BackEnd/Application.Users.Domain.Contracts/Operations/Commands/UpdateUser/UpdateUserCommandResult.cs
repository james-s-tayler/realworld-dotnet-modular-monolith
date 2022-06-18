using Application.Core.DataAccess;
using Application.Users.Domain.Contracts.DTOs;
using MediatR;

namespace Application.Users.Domain.Contracts.Operations.Commands.UpdateUser
{
    public class UpdateUserCommandResult : ContractModel, INotification
    {
        public UserDTO UpdatedUser { get; set; }
    }
}