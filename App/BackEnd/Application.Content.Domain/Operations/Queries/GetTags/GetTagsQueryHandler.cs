using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Content.Domain.Contracts.Operations.Queries.GetTags;
using Application.Content.Domain.Infrastructure.Repositories;
using Application.Core.PipelineBehaviors.OperationResponse;
using MediatR;

namespace Application.Content.Domain.Operations.Queries.GetTags
{
    internal class GetTagsQueryHandler : IRequestHandler<GetTagsQuery, OperationResponse<GetTagsQueryResult>>
    {
        private readonly ITagRepository _tagRepository;
        
        public GetTagsQueryHandler(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public async Task<OperationResponse<GetTagsQueryResult>> Handle(GetTagsQuery request, CancellationToken cancellationToken)
        {
            var tags = await _tagRepository.GetTags();

            return OperationResponseFactory.Success(new GetTagsQueryResult
            {
                Tags = tags.Select(tag => tag.Tag).ToArray()
            });
        }
    }
}