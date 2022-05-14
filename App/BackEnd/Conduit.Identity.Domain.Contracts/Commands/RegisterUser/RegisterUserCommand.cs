using Conduit.Core.Validation.CrossCuttingConcerns.Validation;
using MediatR;

namespace Conduit.Identity.Domain.Contracts.Commands.RegisterUser
{
    public class RegisterUserCommand : IRequest<OperationResponse<RegisterUserResult>>
    {
        public NewUserDTO NewUser { get; set; }
    } 
}