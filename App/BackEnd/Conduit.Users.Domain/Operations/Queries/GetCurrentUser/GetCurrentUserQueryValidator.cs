using System;
using System.Threading;
using System.Threading.Tasks;
using Conduit.Core.Context;
using Conduit.Identity.Domain.Contracts.Queries.GetCurrentUser;
using Conduit.Identity.Domain.Infrastructure.Repositories;
using FluentValidation;

namespace Conduit.Identity.Domain.Operations.Queries.GetCurrentUser
{
    public class GetCurrentUserQueryValidator : AbstractValidator<GetCurrentUserQuery>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserContext _userContext;
        
        public GetCurrentUserQueryValidator(IUserRepository userRepository, 
            IUserContext userContext)
        {
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));

            RuleFor(query => query).MustAsync(UserMustExist).WithMessage($"User {_userContext.UserId} was not found.");
        }
        
        private async Task<bool> UserMustExist(GetCurrentUserQuery query, CancellationToken cancellationToken)
        {
            return await _userRepository.Exists(_userContext.UserId);
        }
    }
}