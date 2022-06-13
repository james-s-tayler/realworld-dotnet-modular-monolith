using System.Threading;
using System.Threading.Tasks;
using Conduit.Social.Domain.Contracts.Queries.GetIsFollowing;
using Conduit.Social.Domain.Infrastructure.Repositories;
using FluentValidation;
using JetBrains.Annotations;

namespace Conduit.Social.Domain.Operations.Queries.GetIsFollowing
{
    internal class GetIsFollowingQueryValidator : AbstractValidator<GetIsFollowingQuery>
    {
        private readonly IUserRepository _userRepository;

        public GetIsFollowingQueryValidator([NotNull] IUserRepository userRepository)
        {
            _userRepository = userRepository;

            RuleFor(query => query).MustAsync(UserMustExist).WithMessage(query => $"User {query.Username} was not found.");
        }
        
        private async Task<bool> UserMustExist(GetIsFollowingQuery query, CancellationToken cancellationToken)
        {
            return await _userRepository.ExistsByUsername(query.Username);
        }
    }
}