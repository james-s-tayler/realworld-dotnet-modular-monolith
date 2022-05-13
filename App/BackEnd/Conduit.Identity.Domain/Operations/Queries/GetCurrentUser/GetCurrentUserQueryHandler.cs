using System.Threading;
using System.Threading.Tasks;
using Conduit.Core.Context;
using Conduit.Core.Validation;
using Conduit.Identity.Domain.Contracts;
using Conduit.Identity.Domain.Contracts.Queries.GetCurrentUser;
using Conduit.Identity.Domain.Infrastructure.Repositories;
using MediatR;

namespace Conduit.Identity.Domain.Operations.Queries.GetCurrentUser
{
    public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, OperationResponse<GetCurrentUserQueryResult>>
    {
        private readonly IUserContext _userContext;
        private readonly IUserRepository _userRepository;

        public GetCurrentUserQueryHandler(IUserContext userContext, 
            IUserRepository userRepository)
        {
            _userContext = userContext;
            _userRepository = userRepository;
        }

        public async Task<OperationResponse<GetCurrentUserQueryResult>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetById(_userContext.UserId);
            
            return new OperationResponse<GetCurrentUserQueryResult>(new GetCurrentUserQueryResult
            {
                CurrentUser = new UserDTO
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    Token = _userContext.Token
                }
            });
        }
    }
}