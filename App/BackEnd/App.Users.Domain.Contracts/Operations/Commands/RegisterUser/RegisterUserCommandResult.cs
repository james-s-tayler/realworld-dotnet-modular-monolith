using System.Diagnostics.CodeAnalysis;
using App.Core.DataAccess;
using App.Users.Domain.Contracts.DTOs;
using MediatR;

namespace App.Users.Domain.Contracts.Operations.Commands.RegisterUser
{
    [ExcludeFromCodeCoverage]
    public class RegisterUserCommandResult : ContractModel, INotification
    {
        public UserDTO RegisteredUser { get; set; }
    }
}