using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Content.Domain.Contracts.DTOs;
using Application.Content.Domain.Contracts.Operations.Queries.ListArticles;
using Application.Content.Domain.Infrastructure.Mappers;
using Application.Content.Domain.Infrastructure.Repositories;
using Application.Content.Domain.Infrastructure.Services;
using Application.Core.PipelineBehaviors.OperationResponse;
using MediatR;

namespace Application.Content.Domain.Operations.Queries.ListArticles
{
    internal class ListArticlesQueryHandler : IRequestHandler<ListArticlesQuery, OperationResponse<ListArticlesQueryResult>>
    {
        private readonly IArticleRepository _articleRepository;
        private readonly ISocialService _socialService;

        public ListArticlesQueryHandler(IArticleRepository articleRepository,
            ISocialService socialService)
        {
            _articleRepository = articleRepository;
            _socialService = socialService;
        }

        public async Task<OperationResponse<ListArticlesQueryResult>> Handle(ListArticlesQuery request, CancellationToken cancellationToken)
        {
            var articles = await _articleRepository.GetAll();

            var articleDtos = new List<SingleArticleDTO>();

            foreach (var article in articles)
            {
                var getProfileQueryResult = await _socialService.GetProfile(article.Author.Username);
                articleDtos.Add(article.ToArticleDTO(getProfileQueryResult.Response.Profile));
            }
            
            return OperationResponseFactory.Success(new ListArticlesQueryResult
            {
                Articles = articleDtos
            });
        }
    }
}