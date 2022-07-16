using System.Threading;
using System.Threading.Tasks;
using App.Core.Context;
using App.Core.PipelineBehaviors.OperationResponse;
using App.Social.Domain.Contracts.Operations.Queries.GetProfile;
using App.Social.Domain.Entities;
using App.Social.Domain.Infrastructure.Mappers;
using App.Social.Domain.Infrastructure.Repositories;
using JetBrains.Annotations;
using MediatR;

namespace App.Social.Domain.Operations.Queries.GetProfile
{
    internal class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, OperationResponse<GetProfileQueryResult>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserContext _context;

        public GetProfileQueryHandler([NotNull] IUserRepository userRepository, 
            [NotNull] IUserContext context)
        {
            _userRepository = userRepository;
            _context = context;
        }

        public async Task<OperationResponse<GetProfileQueryResult>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByUsername(request.Username);
            if(user == null)
                return OperationResponseFactory.NotFound<GetProfileQuery, OperationResponse<GetProfileQueryResult>>(typeof(UserEntity), request.Username);
            
            var isFollowing = _context.IsAuthenticated && await _userRepository.IsFollowing(user.Id);

            return new OperationResponse<GetProfileQueryResult>(new GetProfileQueryResult
            {
                Profile = user.ToProfileDTO(isFollowing)
            });
        }
    }
}