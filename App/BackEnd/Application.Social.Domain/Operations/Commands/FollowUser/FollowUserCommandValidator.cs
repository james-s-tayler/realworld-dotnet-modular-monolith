using System.Threading;
using System.Threading.Tasks;
using Application.Social.Domain.Contracts.Operations.Commands.FollowUser;
using Application.Social.Domain.Infrastructure.Repositories;
using FluentValidation;
using JetBrains.Annotations;

namespace Application.Social.Domain.Operations.Commands.FollowUser
{
    internal class FollowUserCommandValidator : AbstractValidator<FollowUserCommand>
    {
        private readonly IUserRepository _userRepository;

        public FollowUserCommandValidator([NotNull] IUserRepository userRepository)
        {
            _userRepository = userRepository;

            RuleFor(query => query).MustAsync(UserMustExist).WithMessage(query => $"User {query.Username} was not found.");
        }
        
        private async Task<bool> UserMustExist(FollowUserCommand command, CancellationToken cancellationToken)
        {
            return await _userRepository.ExistsByUsername(command.Username);
        }
    }
}