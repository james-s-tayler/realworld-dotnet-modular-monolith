using System;
using System.Threading;
using System.Threading.Tasks;
using Conduit.Core.PipelineBehaviors.OperationResponse;
using Conduit.Users.Domain.Contracts.Queries.GetProfile;
using Conduit.Users.Domain.Infrastructure.Mappers;
using Conduit.Users.Domain.Infrastructure.Repositories;
using Conduit.Users.Domain.Infrastructure.Services;
using MediatR;

namespace Conduit.Users.Domain.Operations.Queries.GetProfile
{
    internal class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, OperationResponse<GetProfileQueryResult>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ISocialService _socialService;

        public GetProfileQueryHandler(IUserRepository userRepository, ISocialService socialService)
        {
            _userRepository = userRepository;
            _socialService = socialService;
        }

        public async Task<OperationResponse<GetProfileQueryResult>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByUsername(request.Username);
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var isFollowing = await _socialService.IsFollowing(user.Id);

            return new OperationResponse<GetProfileQueryResult>(new GetProfileQueryResult
            {
                Profile = user.ToProfileDTO(isFollowing)
            });
        }
    }
}