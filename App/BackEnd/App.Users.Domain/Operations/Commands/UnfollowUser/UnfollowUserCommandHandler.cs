using System.Threading;
using System.Threading.Tasks;
using App.Core.Context;
using App.Core.PipelineBehaviors.OperationResponse;
using App.Users.Domain.Contracts;
using App.Users.Domain.Contracts.Operations.Commands.UnfollowUser;
using App.Users.Domain.Infrastructure.Mappers;
using App.Users.Domain.Infrastructure.Repositories;
using JetBrains.Annotations;
using MediatR;

namespace App.Users.Domain.Operations.Commands.UnfollowUser
{
    internal class UnfollowUserCommandHandler : IRequestHandler<UnfollowUserCommand, OperationResponse<UnfollowUserCommandResult>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserContext _userContext;

        public UnfollowUserCommandHandler([NotNull] IUserRepository userRepository, 
            [NotNull] IUserContext userContext)
        {
            _userRepository = userRepository;
            _userContext = userContext;
        }

        public async Task<OperationResponse<UnfollowUserCommandResult>> Handle(UnfollowUserCommand unfollowUserCommand, CancellationToken cancellationToken)
        {
            var unfollowUserId = await _userRepository.GetByUsername(unfollowUserCommand.Username);
            await _userRepository.UnfollowUser(_userContext.UserId, unfollowUserId.Id);

            return new OperationResponse<UnfollowUserCommandResult>(new UnfollowUserCommandResult
            {
                UnfollowedProfile = UserMapper.ToProfileDTO(unfollowUserId, false)
            });
        }
    }
}