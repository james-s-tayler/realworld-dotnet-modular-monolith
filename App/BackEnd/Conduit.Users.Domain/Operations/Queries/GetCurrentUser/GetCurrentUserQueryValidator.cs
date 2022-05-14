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
            _userRepository = userRepository;
            _userContext = userContext;

            RuleFor(query => query).MustAsync(UserMustExist).WithMessage($"User {_userContext.UserId} was not found.");
        }
        
        private async Task<bool> UserMustExist(GetCurrentUserQuery query, CancellationToken cancellationToken)
        {
            return await _userRepository.Exists(_userContext.UserId);
        }
    }
}