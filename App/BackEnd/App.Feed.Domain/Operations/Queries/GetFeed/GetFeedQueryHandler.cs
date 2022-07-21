using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using App.Content.Domain.Contracts.DTOs;
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

        public GetFeedQueryHandler([NotNull] IUserContext userContext)
        {
            _userContext = userContext;
        }

        public async Task<OperationResponse<GetFeedQueryResult>> Handle(GetFeedQuery request, CancellationToken cancellationToken)
        {
            /*
             * listen to user events: 
             *   - follow user -> insert follower
             *   - unfollow user -> delete follower
             *
             * listen to content events:
             *   - publish article -> populate articles table
             *
             *  follows
             *    - user_id
             *    - following_user_id
             *  articles
             *    - article_id
             *    - user_id
             *    - created_at
             *
             *  select article_id from articles join followers on f.user_id = a.user_id order by created_at desc limit 20 offset 0
             *
             *  bulk-get articles by id -> return
             */

            return OperationResponseFactory.Success(new GetFeedQueryResult
            {
                FeedArticles = new List<SingleArticleDTO>()
            });
        }
    }
}