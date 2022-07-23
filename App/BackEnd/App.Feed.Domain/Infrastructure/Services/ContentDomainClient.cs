using System.Threading.Tasks;
using App.Content.Domain.Contracts.Operations.Queries.GetArticleById;
using App.Core.PipelineBehaviors.OperationResponse;
using MediatR;

namespace App.Feed.Domain.Infrastructure.Services
{
    internal class ContentDomainClient : IContentDomainClient
    {
        private readonly IMediator _mediator;

        public ContentDomainClient(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<OperationResponse<GetArticleByIdQueryResult>> GetArticleById(GetArticleByIdQuery query)
        {
            return await _mediator.Send(query);
        }
    }
}