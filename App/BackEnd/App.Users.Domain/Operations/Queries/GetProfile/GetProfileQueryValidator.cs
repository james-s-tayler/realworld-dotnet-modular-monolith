using System.Threading;
using System.Threading.Tasks;
using App.Core.Context;
using App.Users.Domain.Contracts;
using App.Users.Domain.Contracts.Operations.Queries.GetProfile;
using App.Users.Domain.Infrastructure.Repositories;
using FluentValidation;
using JetBrains.Annotations;

namespace App.Users.Domain.Operations.Queries.GetProfile
{
    internal class GetProfileQueryValidator : AbstractValidator<GetProfileQuery>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserContext _userContext;

        public GetProfileQueryValidator([NotNull] IUserRepository userRepository,
            [NotNull] IUserContext userContext)
        {
            _userContext = userContext;
            _userRepository = userRepository;

            RuleFor(query => query).MustAsync(UserMustExist).WithMessage(query => $"User {query.Username} was not found.");
        }

        private async Task<bool> UserMustExist(GetProfileQuery query, CancellationToken cancellationToken)
        {
            return await _userRepository.ExistsByUsername(query.Username);
        }
    }
}