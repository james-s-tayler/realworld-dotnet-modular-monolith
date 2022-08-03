using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using App.Content.Domain.Contracts.DTOs;
using App.Content.Domain.Contracts.Operations.Queries.GetArticleById;
using App.Core.Context;
using App.Core.PipelineBehaviors.OperationResponse;
using App.Feed.Domain.Contracts.Operations.Queries.GetFeed;
using App.Feed.Domain.Infrastructure.Repositories;
using App.Feed.Domain.Infrastructure.Services;
using JetBrains.Annotations;
using MediatR;

namespace App.Feed.Domain.Operations.Queries.GetFeed
{
    internal class GetFeedQueryHandler : IRequestHandler<GetFeedQuery, OperationResponse<GetFeedQueryResult>>
    {
        private readonly IUserContext _userContext;
        private readonly IArticleRepository _articleRepository;
        private readonly IContentDomainClient _contentDomainClient;

        public GetFeedQueryHandler([NotNull] IUserContext userContext,
            [NotNull] IArticleRepository articleRepository,
            [NotNull] IContentDomainClient contentDomainClient)
        {
            _userContext = userContext;
            _articleRepository = articleRepository;
            _contentDomainClient = contentDomainClient;
        }

        public async Task<OperationResponse<GetFeedQueryResult>> Handle(GetFeedQuery request, CancellationToken cancellationToken)
        {
            var articles = await _articleRepository.GetFeed(_userContext.UserId, request.Limit, request.Offset);
            var feed = new List<SingleArticleDTO>();

            //perf is going to suck doing it this way, batching it would obviously be much better but in the interests of time just doing a simple impl.
            //a further improvement is having background workers pre-populate feeds as articles are published so that retrieval is near instant
            foreach ( var article in articles )
            {
                var getArticleResult = await _contentDomainClient.GetArticleById(new GetArticleByIdQuery { ArticleId = article.ArticleId });
                feed.Add(getArticleResult.Response.Article);
            }

            return OperationResponseFactory.Success(new GetFeedQueryResult
            {
                FeedArticles = feed
            });
        }
    }
}