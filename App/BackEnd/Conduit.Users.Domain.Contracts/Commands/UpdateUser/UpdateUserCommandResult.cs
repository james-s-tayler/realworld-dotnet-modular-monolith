using Application.Core.DataAccess;
using MediatR;

namespace Conduit.Users.Domain.Contracts.Commands.UpdateUser
{
    public class UpdateUserCommandResult : ContractModel, INotification
    {
        public UserDTO UpdatedUser { get; set; }
    }
}