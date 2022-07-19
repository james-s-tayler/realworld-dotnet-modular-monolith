using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using App.Content.Domain.Contracts.DTOs;
using App.Content.Domain.Contracts.Operations.Queries.ListArticles;
using App.Content.Domain.Infrastructure.Mappers;
using App.Content.Domain.Infrastructure.Repositories;
using App.Content.Domain.Infrastructure.Services;
using App.Core.Context;
using App.Core.PipelineBehaviors.OperationResponse;
using JetBrains.Annotations;
using MediatR;

namespace App.Content.Domain.Operations.Queries.ListArticles
{
    internal class ListArticlesQueryHandler : IRequestHandler<ListArticlesQuery, OperationResponse<ListArticlesQueryResult>>
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IUsersService _usersService;
        private readonly IUserRepository _userRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IUserContext _userContext;

        public ListArticlesQueryHandler([NotNull] IArticleRepository articleRepository,
            [NotNull] IUsersService usersService, 
            [NotNull] IUserRepository userRepository, 
            [NotNull] ITagRepository tagRepository, 
            [NotNull] IUserContext userContext)
        {
            _articleRepository = articleRepository;
            _usersService = usersService;
            _userRepository = userRepository;
            _tagRepository = tagRepository;
            _userContext = userContext;
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
                request.Offset,
                _userContext.IsAuthenticated ? _userContext.UserId : null);

            var articleDtos = new List<SingleArticleDTO>();

            //rather than looping some sort of bulk-get would be more performant
            foreach (var article in articles)
            {
                var getProfileQueryResult = await _usersService.GetProfile(article.Author.Username);
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