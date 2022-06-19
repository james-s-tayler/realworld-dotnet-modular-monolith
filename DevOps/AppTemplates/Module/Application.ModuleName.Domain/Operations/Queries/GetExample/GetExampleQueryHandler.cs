using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Context;
using Application.Core.PipelineBehaviors.OperationResponse;
using Application.ModuleName.Domain.Contracts.Operations.Queries.GetExample;
using Application.ModuleName.Domain.Infrastructure.Mappers;
using Application.ModuleName.Domain.Infrastructure.Repositories;
using MediatR;

namespace Application.ModuleName.Domain.Operations.Queries.GetExample
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
            var example = await _exampleRepository.GetById(1);
            if (example == null)
                throw new ArgumentNullException(nameof(example));
            
            return new OperationResponse<GetExampleQueryResult>(new GetExampleQueryResult
            {
                ExampleOutput = example.ToExampleDTO()
            });
        }
    }
}