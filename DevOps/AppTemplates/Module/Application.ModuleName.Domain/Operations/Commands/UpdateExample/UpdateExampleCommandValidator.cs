using System.Threading;
using System.Threading.Tasks;
using Application.ModuleName.Domain.Contracts.DTOs;
using Application.ModuleName.Domain.Contracts.Operations.Commands.UpdateExample;
using Application.ModuleName.Domain.Infrastructure.Repositories;
using FluentValidation;
using JetBrains.Annotations;

namespace Application.ModuleName.Domain.Operations.Commands.UpdateExample
{
    internal class UpdateExampleCommandValidator : AbstractValidator<UpdateExampleCommand>
    {
        private readonly IExampleRepository _exampleRepository;
        public UpdateExampleCommandValidator([NotNull] IExampleRepository exampleRepository)
        {
            _exampleRepository = exampleRepository;

            RuleFor(command => command.ExampleInput)
                .Must(ContainUpdate)
                .WithMessage("At least one property must be updated.");
            RuleFor(command => command)
                .MustAsync(ExampleMustExist)
                .WithMessage(command => $"Example {command.ExampleInput.Id} not found");
        }
        
        //this feels not very future proof?
        private bool ContainUpdate(ExampleDTO updateExample)
        {
            return updateExample.SensitiveValue != null;
        }
        
        private async Task<bool> ExampleMustExist(UpdateExampleCommand command, CancellationToken cancellationToken)
        {
            return await _exampleRepository.Exists(command.ExampleInput.Id);
        }
    }
}