using System;
using System.Threading;
using System.Threading.Tasks;
using Conduit.Core.PipelineBehaviors.OperationResponse;
using Conduit.Users.Domain.Contracts.Queries.GetProfile;
using Conduit.Users.Domain.Infrastructure.Mappers;
using Conduit.Users.Domain.Infrastructure.Repositories;
using MediatR;

namespace Conduit.Users.Domain.Operations.Queries.GetProfile
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
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return new OperationResponse<GetProfileQueryResult>(new GetProfileQueryResult
            {
                Profile = user.ToProfileDTO(false)
            });
        }
    }
}