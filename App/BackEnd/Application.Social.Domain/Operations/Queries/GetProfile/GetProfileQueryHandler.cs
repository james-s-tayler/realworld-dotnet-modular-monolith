using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.PipelineBehaviors.OperationResponse;
using Application.Social.Domain.Contracts.Operations.Queries.GetProfile;
using Application.Social.Domain.Entities;
using Application.Social.Domain.Infrastructure.Mappers;
using Application.Social.Domain.Infrastructure.Repositories;
using MediatR;

namespace Application.Social.Domain.Operations.Queries.GetProfile
{
    internal class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, OperationResponse<GetProfileQueryResult>>
    {
        private readonly IUserRepository _userRepository;

        public GetProfileQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<OperationResponse<GetProfileQueryResult>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByUsername(request.Username);
            if(user == null)
                return OperationResponseFactory.NotFound<GetProfileQuery, OperationResponse<GetProfileQueryResult>>(typeof(UserEntity), request.Username);

            var isFollowing = await _userRepository.IsFollowing(user.Id);

            return new OperationResponse<GetProfileQueryResult>(new GetProfileQueryResult
            {
                Profile = user.ToProfileDTO(isFollowing)
            });
        }
    }
}