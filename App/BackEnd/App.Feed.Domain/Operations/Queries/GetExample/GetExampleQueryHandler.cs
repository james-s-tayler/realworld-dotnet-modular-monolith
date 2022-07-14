using System.Threading;
using System.Threading.Tasks;
using App.Core.Context;
using App.Core.PipelineBehaviors.OperationResponse;
using App.Feed.Domain.Contracts.Operations.Queries.GetExample;
using App.Feed.Domain.Entities;
using App.Feed.Domain.Infrastructure.Mappers;
using App.Feed.Domain.Infrastructure.Repositories;
using MediatR;

namespace App.Feed.Domain.Operations.Queries.GetExample
{
    internal class GetExampleQueryHandler : IRequestHandler<GetExampleQuery, OperationResponse<GetExampleQueryResult>>
    {
        private readonly IUserContext _userContext;
        private readonly IExampleRepository _exampleRepository;

        public GetExampleQueryHandler(IUserContext userContext, 
            IExampleRepository exampleRepository)
        {
            _userContext = userContext;
            _exampleRepository = exampleRepository;
        }

        public async Task<OperationResponse<GetExampleQueryResult>> Handle(GetExampleQuery request, CancellationToken cancellationToken)
        {
            var example = await _exampleRepository.GetById(request.Id);
            if (example == null)
                return OperationResponseFactory.NotFound<GetExampleQuery, OperationResponse<GetExampleQueryResult>>(typeof(ExampleEntity), request.Id);

            return OperationResponseFactory.Success(new GetExampleQueryResult
            {
                ExampleOutput = example.ToExampleDTO()
            });
        }
    }
}