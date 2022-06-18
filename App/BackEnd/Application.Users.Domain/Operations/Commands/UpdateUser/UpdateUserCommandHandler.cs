using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Context;
using Application.Core.PipelineBehaviors.OperationResponse;
using Application.Users.Domain.Contracts.Operations.Commands.UpdateUser;
using Application.Users.Domain.Infrastructure.Mappers;
using Application.Users.Domain.Infrastructure.Repositories;
using JetBrains.Annotations;
using MediatR;

namespace Application.Users.Domain.Operations.Commands.UpdateUser
{
    internal class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, OperationResponse<UpdateUserCommandResult>>
    {
        private readonly IUserContext _userContext;
        private readonly IUserRepository _userRepository;

        public UpdateUserCommandHandler([NotNull] IUserContext userContext,
            [NotNull] IUserRepository userRepository)
        {
            _userContext = userContext;
            _userRepository = userRepository;
        }

        public async Task<OperationResponse<UpdateUserCommandResult>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetById(_userContext.UserId);
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            
            user.Username = request.UpdateUser.Username ?? user.Username;
            user.Email = request.UpdateUser.Email ?? user.Email;
            user.Image = request.UpdateUser.Image ?? user.Image; 
            user.Bio = request.UpdateUser.Bio ?? user.Bio;

            await _userRepository.Update(user);

            return new OperationResponse<UpdateUserCommandResult>(new UpdateUserCommandResult
            {
                UpdatedUser = user.ToUserDTO(_userContext.Token) 
            });
        }
    }
}