using System.Threading;
using System.Threading.Tasks;
using App.Content.Domain.Contracts.Operations.Commands.UnfavoriteArticle;
using App.Content.Domain.Entities;
using App.Content.Domain.Infrastructure.Mappers;
using App.Content.Domain.Infrastructure.Repositories;
using App.Content.Domain.Infrastructure.Services;
using App.Core.PipelineBehaviors.OperationResponse;
using JetBrains.Annotations;
using MediatR;

namespace App.Content.Domain.Operations.Commands.UnfavoriteArticle
{
    internal class UnfavoriteArticleCommandHandler : IRequestHandler<UnfavoriteArticleCommand, OperationResponse<UnfavoriteArticleCommandResult>>
    {
        private readonly ISocialService _socialService;
        private readonly IArticleRepository _articleRepository;

        public UnfavoriteArticleCommandHandler([NotNull] IArticleRepository articleRepository, 
            [NotNull] ISocialService socialService)
        {
            _articleRepository = articleRepository;
            _socialService = socialService;
        }

        public async Task<OperationResponse<UnfavoriteArticleCommandResult>> Handle(UnfavoriteArticleCommand request, CancellationToken cancellationToken)
        {
            if (!await _articleRepository.ExistsBySlug(request.Slug))
                return OperationResponseFactory.NotFound<UnfavoriteArticleCommand, OperationResponse<UnfavoriteArticleCommandResult>>(typeof(ArticleEntity), request.Slug);

            await _articleRepository.UnfavoriteArticle(request.Slug);
            var article = await _articleRepository.GetBySlug(request.Slug);

            var getProfileQueryResult = await _socialService.GetProfile(article.Author.Username);
            var authorProfile = getProfileQueryResult.Response.Profile;

            return OperationResponseFactory.Success(new UnfavoriteArticleCommandResult
            {
                Article = article.ToArticleDTO(authorProfile)
            });
        }
    }
}