using System.Threading;
using System.Threading.Tasks;
using Conduit.Core.Context;
using Conduit.Core.PipelineBehaviors.OperationResponse;
using Conduit.Social.Domain.Contracts.Queries.GetIsFollowing;
using Conduit.Social.Domain.Infrastructure.Repositories;
using JetBrains.Annotations;
using MediatR;

namespace Conduit.Social.Domain.Operations.Queries.GetIsFollowing
{
    internal class GetIsFollowingQueryHandler : IRequestHandler<GetIsFollowingQuery, OperationResponse<GetIsFollowingQueryResult>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserContext _userContext;

        public GetIsFollowingQueryHandler([NotNull] IUserRepository userRepository, [NotNull] IUserContext userContext)
        {
            _userRepository = userRepository;
            _userContext = userContext;
        }

        public async Task<OperationResponse<GetIsFollowingQueryResult>> Handle(GetIsFollowingQuery request, CancellationToken cancellationToken)
        {
            var followingUser = await _userRepository.GetByUsername(request.Username);
            var isFollowing = await _userRepository.IsFollowing(_userContext.UserId, followingUser.Id);

            return new OperationResponse<GetIsFollowingQueryResult>(new GetIsFollowingQueryResult
            {
                Following = isFollowing
            });
        }
    }
}