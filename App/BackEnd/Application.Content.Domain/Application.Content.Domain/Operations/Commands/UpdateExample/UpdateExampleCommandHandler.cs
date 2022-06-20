using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Context;
using Application.Core.PipelineBehaviors.OperationResponse;
using Application.Content.Domain.Contracts.Operations.Commands.UpdateExample;
using Application.Content.Domain.Infrastructure.Mappers;
using Application.Content.Domain.Infrastructure.Repositories;
using JetBrains.Annotations;
using MediatR;

namespace Application.Content.Domain.Operations.Commands.UpdateExample
{
    internal class
        UpdateExampleCommandHandler : IRequestHandler<UpdateExampleCommand,
            OperationResponse<UpdateExampleCommandResult>>
    {
        private readonly IUserContext _userContext;
        private readonly IExampleRepository _exampleRepository;

        public UpdateExampleCommandHandler([NotNull] IUserContext userContext,
            [NotNull] IExampleRepository exampleRepository)
        {
            _userContext = userContext;
            _exampleRepository = exampleRepository;
        }

        public async Task<OperationResponse<UpdateExampleCommandResult>> Handle(UpdateExampleCommand request,
            CancellationToken cancellationToken)
        {
            var example = await _exampleRepository.GetById(request.ExampleInput.Id);
            if (example == null)
                throw new ArgumentNullException(nameof(example));

            example.Something = request.ExampleInput.SensitiveValue;
            await _exampleRepository.Update(example);

            return new OperationResponse<UpdateExampleCommandResult>(new UpdateExampleCommandResult
            {
                ExampleOutput = example.ToExampleDTO()
            });
        }
    }
}