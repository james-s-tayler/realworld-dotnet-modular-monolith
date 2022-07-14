using System.Threading;
using System.Threading.Tasks;
using App.Content.Domain.Contracts.Operations.Queries.GetSingleArticle;
using App.Content.Domain.Entities;
using App.Content.Domain.Infrastructure.Mappers;
using App.Content.Domain.Infrastructure.Repositories;
using App.Content.Domain.Infrastructure.Services;
using App.Core.PipelineBehaviors.OperationResponse;
using MediatR;

namespace App.Content.Domain.Operations.Queries.GetSingleArticle
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