using System.Threading;
using System.Threading.Tasks;
using App.Content.Domain.Contracts.Operations.Commands.FavoriteArticle;
using App.Content.Domain.Entities;
using App.Content.Domain.Infrastructure.Mappers;
using App.Content.Domain.Infrastructure.Repositories;
using App.Content.Domain.Infrastructure.Services;
using App.Core.Context;
using App.Core.PipelineBehaviors.OperationResponse;
using JetBrains.Annotations;
using MediatR;

namespace App.Content.Domain.Operations.Commands.FavoriteArticle
{
    internal class FavoriteArticleCommandHandler : IRequestHandler<FavoriteArticleCommand, OperationResponse<FavoriteArticleCommandResult>>
    {
        private readonly IUsersService _usersService;
        private readonly IArticleRepository _articleRepository;
        private readonly IUserContext _userContext;

        public FavoriteArticleCommandHandler([NotNull] IArticleRepository articleRepository, 
            [NotNull] IUsersService usersService, 
            [NotNull] IUserContext userContext)
        {
            _articleRepository = articleRepository;
            _usersService = usersService;
            _userContext = userContext;
        }

        public async Task<OperationResponse<FavoriteArticleCommandResult>> Handle(FavoriteArticleCommand request, CancellationToken cancellationToken)
        {
            if (!await _articleRepository.ExistsBySlug(request.Slug))
                return OperationResponseFactory.NotFound<FavoriteArticleCommand, OperationResponse<FavoriteArticleCommandResult>>(typeof(ArticleEntity), request.Slug);

            await _articleRepository.FavoriteArticle(request.Slug, _userContext.UserId);
            var article = await _articleRepository.GetBySlug(request.Slug, _userContext.UserId);

            var getProfileQueryResult = await _usersService.GetProfile(article.Author.Username);
            var authorProfile = getProfileQueryResult.Response.Profile;

            return OperationResponseFactory.Success(new FavoriteArticleCommandResult
            {
                Article = article.ToArticleDTO(authorProfile)
            });
        }
    }
}