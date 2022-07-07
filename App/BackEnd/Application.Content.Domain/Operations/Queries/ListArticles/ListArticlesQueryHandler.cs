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
            if (await UserNotFound(request.AuthorUsername) ||
                await UserNotFound(request.FavoritedByUsername) ||
                await TagNotFound(request.Tag))
            {
                return OperationResponseFactory.Success(new ListArticlesQueryResult
                {
                    Articles = new List<SingleArticleDTO>()
                });   
            }
           
            var articles = await _articleRepository.GetByFilters(
                request.AuthorUsername, 
                request.FavoritedByUsername, 
                request.Tag,
                request.Limit,
                request.Offset);

            var articleDtos = new List<SingleArticleDTO>();

            //rather than looping some sort of bulk-get would be more performant
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

        private async Task<bool> UserNotFound(string authorUsername)
        {
            return !string.IsNullOrEmpty(authorUsername) && !await _userRepository.ExistsByUsername(authorUsername);
        }

        private async Task<bool> TagNotFound(string tag)
        {
            return !string.IsNullOrEmpty(tag) && !await _tagRepository.Exists(tag);
        }
    }
}