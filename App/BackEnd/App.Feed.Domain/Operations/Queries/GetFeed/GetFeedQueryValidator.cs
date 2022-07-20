using System.Threading;
using System.Threading.Tasks;
using App.Feed.Domain.Contracts.Operations.Queries.GetFeed;
using App.Feed.Domain.Infrastructure.Repositories;
using FluentValidation;
using JetBrains.Annotations;

namespace App.Feed.Domain.Operations.Queries.GetFeed
{
    internal class GetFeedQueryValidator : AbstractValidator<GetFeedQuery>
    {
        private readonly IExampleRepository _exampleRepository;
        public GetFeedQueryValidator([NotNull] IExampleRepository exampleRepository)
        {
            _exampleRepository = exampleRepository;

            RuleFor(command => command)
                .MustAsync(ExampleMustExist)
                .WithMessage(command => $"Example {command.ExampleInput.Id} not found");
        }

        private async Task<bool> ExampleMustExist(GetFeedQuery command, CancellationToken cancellationToken)
        {
            return await _exampleRepository.Exists(command.ExampleInput.Id);
        }
    }
}