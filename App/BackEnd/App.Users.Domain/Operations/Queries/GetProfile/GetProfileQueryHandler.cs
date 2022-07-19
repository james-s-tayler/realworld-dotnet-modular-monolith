using System.Threading;
using System.Threading.Tasks;
using App.Core.Context;
using App.Core.PipelineBehaviors.OperationResponse;
using App.Users.Domain.Contracts;
using App.Users.Domain.Contracts.Operations.Queries.GetProfile;
using App.Users.Domain.Entities;
using App.Users.Domain.Infrastructure.Mappers;
using App.Users.Domain.Infrastructure.Repositories;
using JetBrains.Annotations;
using MediatR;

namespace App.Users.Domain.Operations.Queries.GetProfile
{
    internal class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, OperationResponse<GetProfileQueryResult>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserContext _userContext;

        public GetProfileQueryHandler([NotNull] IUserRepository userRepository, 
            [NotNull] IUserContext userContext)
        {
            _userRepository = userRepository;
            _userContext = userContext;
        }

        public async Task<OperationResponse<GetProfileQueryResult>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByUsername(request.Username);
            if(user == null)
                return OperationResponseFactory.NotFound<GetProfileQuery, OperationResponse<GetProfileQueryResult>>(typeof(UserEntity), request.Username);
            
            var isFollowing = _userContext.IsAuthenticated && await _userRepository.IsFollowing(_userContext.UserId, user.Id);

            return new OperationResponse<GetProfileQueryResult>(new GetProfileQueryResult
            {
                Profile = UserMapper.ToProfileDTO(user, isFollowing)
            });
        }
    }
}