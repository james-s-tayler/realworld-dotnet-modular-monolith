using System.Threading;
using System.Threading.Tasks;
using Application.Core.Context;
using Conduit.Users.Domain.Contracts.Queries.GetCurrentUser;
using Conduit.Users.Domain.Infrastructure.Repositories;
using FluentValidation;
using JetBrains.Annotations;

namespace Conduit.Users.Domain.Operations.Queries.GetCurrentUser
{
    internal class GetCurrentUserQueryValidator : AbstractValidator<GetCurrentUserQuery>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserContext _userContext;
        
        public GetCurrentUserQueryValidator([NotNull] IUserRepository userRepository, 
            [NotNull] IUserContext userContext)
        {
            _userContext = userContext;
            _userRepository = userRepository;

            RuleFor(query => query).MustAsync(UserMustExist).WithMessage($"User {_userContext.UserId} was not found.");
        }
        
        private async Task<bool> UserMustExist(GetCurrentUserQuery query, CancellationToken cancellationToken)
        {
            return await _userRepository.Exists(_userContext.UserId);
        }
    }
}