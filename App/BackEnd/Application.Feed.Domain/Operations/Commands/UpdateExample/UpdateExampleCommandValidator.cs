using System.Threading;
using System.Threading.Tasks;
using Application.Feed.Domain.Contracts.DTOs;
using Application.Feed.Domain.Contracts.Operations.Commands.UpdateExample;
using Application.Feed.Domain.Infrastructure.Repositories;
using FluentValidation;
using JetBrains.Annotations;

namespace Application.Feed.Domain.Operations.Commands.UpdateExample
{
    internal class UpdateExampleCommandValidator : AbstractValidator<UpdateExampleCommand>
    {
        private readonly IExampleRepository _exampleRepository;
        public UpdateExampleCommandValidator([NotNull] IExampleRepository exampleRepository)
        {
            _exampleRepository = exampleRepository;

            RuleFor(command => command)
                .MustAsync(ExampleMustExist)
                .WithMessage(command => $"Example {command.ExampleInput.Id} not found");
        }

        private async Task<bool> ExampleMustExist(UpdateExampleCommand command, CancellationToken cancellationToken)
        {
            return await _exampleRepository.Exists(command.ExampleInput.Id);
        }
    }
}