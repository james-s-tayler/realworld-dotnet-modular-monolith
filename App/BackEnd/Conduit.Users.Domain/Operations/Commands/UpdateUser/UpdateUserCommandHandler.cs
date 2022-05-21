using System;
using System.Threading;
using System.Threading.Tasks;
using Conduit.Core.Context;
using Conduit.Core.PipelineBehaviors;
using Conduit.Identity.Domain.Contracts.Commands.UpdateUser;
using Conduit.Identity.Domain.Infrastructure.Mappers;
using Conduit.Identity.Domain.Infrastructure.Repositories;
using JetBrains.Annotations;
using MediatR;

namespace Conduit.Identity.Domain.Operations.Commands.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, OperationResponse<UpdateUserResult>>
    {
        private readonly IUserContext _userContext;
        private readonly IUserRepository _userRepository;

        public UpdateUserCommandHandler([NotNull] IUserContext userContext,
            [NotNull] IUserRepository userRepository)
        {
            _userContext = userContext;
            _userRepository = userRepository;
        }

        public async Task<OperationResponse<UpdateUserResult>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetById(_userContext.UserId);
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            
            user.Username = request.UpdateUser.Username ?? user.Username;
            user.Email = request.UpdateUser.Email ?? user.Email;
            user.Image = request.UpdateUser.Image ?? user.Image; 
            user.Bio = request.UpdateUser.Bio ?? user.Bio;

            await _userRepository.Update(user);

            return new OperationResponse<UpdateUserResult>(new UpdateUserResult
            {
                UpdatedUser = user.ToUserDTO(_userContext.Token) 
            });
        }
    }
}