using System.Threading;
using System.Threading.Tasks;
using App.Core.Context;
using App.Core.PipelineBehaviors.OperationResponse;
using App.Feed.Domain.Contracts.Operations.Queries.GetFeed;
using App.Feed.Domain.Infrastructure.Mappers;
using App.Feed.Domain.Infrastructure.Repositories;
using JetBrains.Annotations;
using MediatR;

namespace App.Feed.Domain.Operations.Queries.GetFeed
{
    internal class GetFeedQueryHandler : IRequestHandler<GetFeedQuery, OperationResponse<GetFeedQueryResult>>
    {
        private readonly IUserContext _userContext;
        private readonly IExampleRepository _exampleRepository;

        public GetFeedQueryHandler([NotNull] IUserContext userContext,
            [NotNull] IExampleRepository exampleRepository)
        {
            _userContext = userContext;
            _exampleRepository = exampleRepository;
        }

        public async Task<OperationResponse<GetFeedQueryResult>> Handle(GetFeedQuery request, CancellationToken cancellationToken)
        {
            var example = await _exampleRepository.GetById(request.ExampleInput.Id);

            example.Something = request.ExampleInput.SensitiveValue;
            await _exampleRepository.Update(example);

            return OperationResponseFactory.Success(new GetFeedQueryResult
            {
                ExampleOutput = example.ToExampleDTO()
            });
        }
    }
}