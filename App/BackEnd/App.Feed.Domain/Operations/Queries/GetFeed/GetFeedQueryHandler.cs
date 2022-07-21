using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using App.Content.Domain.Contracts.DTOs;
using App.Core.Context;
using App.Core.PipelineBehaviors.OperationResponse;
using App.Feed.Domain.Contracts.Operations.Queries.GetFeed;
using App.Feed.Domain.Infrastructure.Repositories;
using JetBrains.Annotations;
using MediatR;

namespace App.Feed.Domain.Operations.Queries.GetFeed
{
    internal class GetFeedQueryHandler : IRequestHandler<GetFeedQuery, OperationResponse<GetFeedQueryResult>>
    {
        private readonly IUserContext _userContext;
        private readonly IArticleRepository _articleRepository;

        public GetFeedQueryHandler([NotNull] IUserContext userContext,
            [NotNull] IArticleRepository articleRepository)
        {
            _userContext = userContext;
            _articleRepository = articleRepository;
        }

        public async Task<OperationResponse<GetFeedQueryResult>> Handle(GetFeedQuery request, CancellationToken cancellationToken)
        {
            var articles = await _articleRepository.GetFeed(_userContext.UserId, request.Limit, request.Offset);
            
            return OperationResponseFactory.Success(new GetFeedQueryResult
            {
                FeedArticles = new List<SingleArticleDTO>()
            });
        }
    }
}