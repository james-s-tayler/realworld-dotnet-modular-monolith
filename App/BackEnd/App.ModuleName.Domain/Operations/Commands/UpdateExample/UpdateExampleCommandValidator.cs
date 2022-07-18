using System.Threading;
using System.Threading.Tasks;
using App.ModuleName.Domain.Contracts.Operations.Commands.UpdateExample;
using App.ModuleName.Domain.Infrastructure.Repositories;
using FluentValidation;
using JetBrains.Annotations;

namespace App.ModuleName.Domain.Operations.Commands.UpdateExample
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