using System.Threading;
using System.Threading.Tasks;
using Application.Content.Domain.Contracts.Operations.Commands.UnfavoriteArticle;
using Application.Content.Domain.Entities;
using Application.Content.Domain.Infrastructure.Mappers;
using Application.Content.Domain.Infrastructure.Repositories;
using Application.Content.Domain.Infrastructure.Services;
using Application.Core.Context;
using Application.Core.PipelineBehaviors.OperationResponse;
using JetBrains.Annotations;
using MediatR;

namespace Application.Content.Domain.Operations.Commands.UnfavoriteArticle
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