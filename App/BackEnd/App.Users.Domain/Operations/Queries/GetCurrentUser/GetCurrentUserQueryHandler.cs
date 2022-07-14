using System;
using System.Threading;
using System.Threading.Tasks;
using App.Core.Context;
using App.Core.PipelineBehaviors.OperationResponse;
using App.Users.Domain.Contracts.Operations.Queries.GetCurrentUser;
using App.Users.Domain.Infrastructure.Mappers;
using App.Users.Domain.Infrastructure.Repositories;
using MediatR;

namespace App.Users.Domain.Operations.Queries.GetCurrentUser
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