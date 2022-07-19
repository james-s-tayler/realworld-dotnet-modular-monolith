using System.Threading;
using System.Threading.Tasks;
using App.Content.Domain.Contracts.Operations.Queries.GetSingleArticle;
using App.Content.Domain.Entities;
using App.Content.Domain.Infrastructure.Mappers;
using App.Content.Domain.Infrastructure.Repositories;
using App.Content.Domain.Infrastructure.Services;
using App.Core.Context;
using App.Core.PipelineBehaviors.OperationResponse;
using JetBrains.Annotations;
using MediatR;

namespace App.Content.Domain.Operations.Queries.GetSingleArticle
{
    internal class GetSingleArticleQueryHandler : IRequestHandler<GetSingleArticleQuery, OperationResponse<GetSingleArticleQueryResult>>
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IUsersService _usersService;
        private readonly IUserContext _userContext;

        public GetSingleArticleQueryHandler([NotNull] IArticleRepository articleRepository,
            [NotNull] IUsersService usersService, 
            [NotNull] IUserContext userContext)
        {
            _articleRepository = articleRepository;
            _usersService = usersService;
            _userContext = userContext;
        }

        public async Task<OperationResponse<GetSingleArticleQueryResult>> Handle(GetSingleArticleQuery request, CancellationToken cancellationToken)
        {
            var article = await _articleRepository.GetBySlug(request.Slug, _userContext.UserId);
            if (article == null)
                return OperationResponseFactory.NotFound<GetSingleArticleQuery, OperationResponse<GetSingleArticleQueryResult>>(typeof(ArticleEntity), request.Slug);

            //what if this operation fails???
            var getProfileQueryResult = await _usersService.GetProfile(article.Author.Username);
            var authorProfile = getProfileQueryResult.Response.Profile;
            
            return OperationResponseFactory.Success(new GetSingleArticleQueryResult
            {
                Article = article.ToArticleDTO(authorProfile)
            });
        }
    }
}