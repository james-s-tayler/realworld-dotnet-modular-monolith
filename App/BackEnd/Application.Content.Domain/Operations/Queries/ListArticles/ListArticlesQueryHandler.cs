using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Content.Domain.Contracts.DTOs;
using Application.Content.Domain.Contracts.Operations.Queries.ListArticles;
using Application.Content.Domain.Entities;
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
        private readonly IUserRepository _userRepository;
        private readonly ITagRepository _tagRepository;

        public ListArticlesQueryHandler(IArticleRepository articleRepository,
            ISocialService socialService, 
            IUserRepository userRepository, 
            ITagRepository tagRepository)
        {
            _articleRepository = articleRepository;
            _socialService = socialService;
            _userRepository = userRepository;
            _tagRepository = tagRepository;
        }

        public async Task<OperationResponse<ListArticlesQueryResult>> Handle(ListArticlesQuery request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(request.AuthorUsername) && !await _userRepository.ExistsByUsername(request.AuthorUsername))
                return OperationResponseFactory.NotFound<ListArticlesQuery, OperationResponse<ListArticlesQueryResult>>(typeof(UserEntity), request.AuthorUsername);
            
            if (!string.IsNullOrEmpty(request.FavoritedByUsername) && !await _userRepository.ExistsByUsername(request.FavoritedByUsername))
                return OperationResponseFactory.NotFound<ListArticlesQuery, OperationResponse<ListArticlesQueryResult>>(typeof(UserEntity), request.FavoritedByUsername);

            if (!string.IsNullOrEmpty(request.Tag) && !await _tagRepository.Exists(request.Tag))
                return OperationResponseFactory.NotFound<ListArticlesQuery, OperationResponse<ListArticlesQueryResult>>(typeof(TagEntity), request.Tag);
            
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