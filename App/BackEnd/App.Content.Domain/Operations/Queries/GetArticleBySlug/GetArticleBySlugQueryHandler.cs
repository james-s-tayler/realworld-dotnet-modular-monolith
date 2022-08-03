using System.Threading;
using System.Threading.Tasks;
using App.Content.Domain.Contracts.Operations.Queries.GetArticleBySlug;
using App.Content.Domain.Entities;
using App.Content.Domain.Infrastructure.Mappers;
using App.Content.Domain.Infrastructure.Repositories;
using App.Content.Domain.Infrastructure.Services;
using App.Core.Context;
using App.Core.PipelineBehaviors.OperationResponse;
using JetBrains.Annotations;
using MediatR;

namespace App.Content.Domain.Operations.Queries.GetArticleBySlug
{
    internal class GetArticleBySlugQueryHandler : IRequestHandler<GetArticleBySlugQuery, OperationResponse<GetArticleBySlugQueryResult>>
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IUsersService _usersService;
        private readonly IUserContext _userContext;

        public GetArticleBySlugQueryHandler([NotNull] IArticleRepository articleRepository,
            [NotNull] IUsersService usersService,
            [NotNull] IUserContext userContext)
        {
            _articleRepository = articleRepository;
            _usersService = usersService;
            _userContext = userContext;
        }

        public async Task<OperationResponse<GetArticleBySlugQueryResult>> Handle(GetArticleBySlugQuery request, CancellationToken cancellationToken)
        {
            var article = await _articleRepository.GetBySlug(request.Slug, _userContext.UserId);
            if ( article == null )
                return OperationResponseFactory.NotFound<GetArticleBySlugQuery, OperationResponse<GetArticleBySlugQueryResult>>(typeof(ArticleEntity), request.Slug);

            //what if this operation fails???
            var getProfileQueryResult = await _usersService.GetProfile(article.Author.Username);
            var authorProfile = getProfileQueryResult.Response.Profile;

            return OperationResponseFactory.Success(new GetArticleBySlugQueryResult
            {
                Article = article.ToArticleDTO(authorProfile)
            });
        }
    }
}