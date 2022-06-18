using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Context;
using Application.Core.PipelineBehaviors.OperationResponse;
using Application.Users.Domain.Contracts.Operations.Queries.GetCurrentUser;
using Application.Users.Domain.Infrastructure.Mappers;
using Application.Users.Domain.Infrastructure.Repositories;
using MediatR;

namespace Application.Users.Domain.Operations.Queries.GetCurrentUser
{
    internal class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, OperationResponse<GetCurrentUserQueryResult>>
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
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            
            return new OperationResponse<GetCurrentUserQueryResult>(new GetCurrentUserQueryResult
            {
                CurrentUser = user.ToUserDTO(_userContext.Token)
            });
        }
    }
}