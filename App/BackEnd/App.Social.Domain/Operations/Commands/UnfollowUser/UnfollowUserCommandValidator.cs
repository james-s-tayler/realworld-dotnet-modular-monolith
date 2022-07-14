using System.Threading;
using System.Threading.Tasks;
using App.Social.Domain.Contracts.Operations.Commands.UnfollowUser;
using Application.Social.Domain.Infrastructure.Repositories;
using FluentValidation;
using JetBrains.Annotations;

namespace Application.Social.Domain.Operations.Commands.UnfollowUser
{
    internal class UnfollowUserCommandValidator : AbstractValidator<UnfollowUserCommand>
    {
        private readonly IUserRepository _userRepository;

        public UnfollowUserCommandValidator([NotNull] IUserRepository userRepository)
        {
            _userRepository = userRepository;

            RuleFor(query => query).MustAsync(UserMustExist).WithMessage(query => $"User {query.Username} was not found.");
        }
        
        private async Task<bool> UserMustExist(UnfollowUserCommand command, CancellationToken cancellationToken)
        {
            return await _userRepository.ExistsByUsername(command.Username);
        }
    }
}