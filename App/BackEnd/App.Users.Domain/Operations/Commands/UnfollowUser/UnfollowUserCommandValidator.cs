using System.Threading;
using System.Threading.Tasks;
using App.Core.Context;
using App.Users.Domain.Contracts.Operations.Commands.UnfollowUser;
using App.Users.Domain.Infrastructure.Repositories;
using FluentValidation;
using JetBrains.Annotations;

namespace App.Users.Domain.Operations.Commands.UnfollowUser
{
    internal class UnfollowUserCommandValidator : AbstractValidator<UnfollowUserCommand>
    {
        private readonly IUserContext _userContext;
        private readonly IUserRepository _userRepository;

        public UnfollowUserCommandValidator([NotNull] IUserRepository userRepository, IUserContext userContext)
        {
            _userRepository = userRepository;
            _userContext = userContext;

            RuleFor(query => query)
                .MustAsync(UserMustBeFollowed)
                .WithName("username")
                .WithMessage(query => $"User {query.Username} was not found.");
        }

        private async Task<bool> UserMustBeFollowed(UnfollowUserCommand command, CancellationToken cancellationToken)
        {
            var followingUser = await _userRepository.GetByUsername(command.Username);
            if ( followingUser == null )
                return false;

            return await _userRepository.IsFollowing(_userContext.UserId, followingUser.Id);
        }
    }
}