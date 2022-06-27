using System.Threading;
using System.Threading.Tasks;
using Application.Content.Domain.Contracts.Operations.Queries.GetSingleArticle;
using Application.Content.Domain.Entities;
using Application.Content.Domain.Infrastructure.Mappers;
using Application.Content.Domain.Infrastructure.Repositories;
using Application.Content.Domain.Infrastructure.Services;
using Application.Core.Context;
using Application.Core.PipelineBehaviors.OperationResponse;
using Application.Social.Domain.Contracts.Operations.Queries.GetProfile;
using MediatR;

namespace Application.Content.Domain.Operations.Queries.GetSingleArticle
{
    internal class GetSingleArticleQueryHandler : IRequestHandler<GetSingleArticleQuery, OperationResponse<GetSingleArticleQueryResult>>
    {
        private readonly IArticleRepository _articleRepository;
        private readonly ISocialService _socialService;

        public GetSingleArticleQueryHandler(IArticleRepository articleRepository,
            ISocialService socialService)
        {
            _articleRepository = articleRepository;
            _socialService = socialService;
        }

        public async Task<OperationResponse<GetSingleArticleQueryResult>> Handle(GetSingleArticleQuery request, CancellationToken cancellationToken)
        {
            var article = await _articleRepository.GetBySlug(request.Slug);
            if (article == null)
                return OperationResponseFactory.NotFound<GetSingleArticleQuery, OperationResponse<GetSingleArticleQueryResult>>(typeof(ArticleEntity), request.Slug);

            var getProfileQueryResult = await _socialService.GetProfile(article.Author.Username);
            var authorProfile = getProfileQueryResult.Response.Profile;
            //what if result is not success???

            return OperationResponseFactory.Success(new GetSingleArticleQueryResult
            {
                Article = article.ToArticleDTO(authorProfile)
            });
        }
    }
}