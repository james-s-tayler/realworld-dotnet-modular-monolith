using System.Threading;
using System.Threading.Tasks;
using Application.Content.Domain.Contracts.Operations.Queries.GetExample;
using Application.Content.Domain.Infrastructure.Repositories;
using FluentValidation;
using JetBrains.Annotations;

namespace Application.Content.Domain.Operations.Queries.GetExample
{
    internal class GetExampleQueryValidator : AbstractValidator<GetExampleQuery>
    {
        private readonly IExampleRepository _exampleRepository;

        public GetExampleQueryValidator([NotNull] IExampleRepository exampleRepository)
        {
            _exampleRepository = exampleRepository;

            RuleFor(query => query).MustAsync(ExampleMustExist)
                .WithMessage(query => $"Example {query.Id} was not found.");
        }

        private async Task<bool> ExampleMustExist(GetExampleQuery query, CancellationToken cancellationToken)
        {
            return await _exampleRepository.Exists(query.Id);
        }
    }
}