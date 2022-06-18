using System.Threading;
using System.Threading.Tasks;
using Application.Core.Context;
using Application.Social.Domain.Contracts.Operations.Queries.GetProfile;
using Application.Social.Domain.Infrastructure.Repositories;
using FluentValidation;
using JetBrains.Annotations;

namespace Application.Social.Domain.Operations.Queries.GetProfile
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

            RuleFor(query => query).MustAsync(UserMustExist).WithMessage($"User {_userContext.Username} was not found.");
        }
        
        private async Task<bool> UserMustExist(GetProfileQuery query, CancellationToken cancellationToken)
        {
            return await _userRepository.ExistsByUsername(query.Username);
        }
    }
}