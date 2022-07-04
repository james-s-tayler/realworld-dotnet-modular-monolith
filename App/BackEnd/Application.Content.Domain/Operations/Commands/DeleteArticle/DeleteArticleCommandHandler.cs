using System.Threading;
using System.Threading.Tasks;
using Application.Content.Domain.Contracts.Operations.Commands.DeleteArticle;
using Application.Content.Domain.Contracts.Operations.Commands.FavoriteArticle;
using Application.Content.Domain.Entities;
using Application.Content.Domain.Infrastructure.Mappers;
using Application.Content.Domain.Infrastructure.Repositories;
using Application.Content.Domain.Infrastructure.Services;
using Application.Core.PipelineBehaviors.OperationResponse;
using JetBrains.Annotations;
using MediatR;

namespace Application.Content.Domain.Operations.Commands.DeleteArticle
{
    internal class DeleteArticleCommandHandler : IRequestHandler<DeleteArticleCommand, OperationResponse<DeleteArticleCommandResult>>
    {
        private readonly IArticleRepository _articleRepository;

        public DeleteArticleCommandHandler([NotNull] IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public async Task<OperationResponse<DeleteArticleCommandResult>> Handle(DeleteArticleCommand request, CancellationToken cancellationToken)
        {
            if (!await _articleRepository.ExistsBySlug(request.Slug))
                return OperationResponseFactory.NotFound<DeleteArticleCommand, OperationResponse<DeleteArticleCommandResult>>(typeof(ArticleEntity), request.Slug);

            var article = await _articleRepository.GetBySlug(request.Slug);
            await _articleRepository.Delete(article.Id);

            return OperationResponseFactory.Success(new DeleteArticleCommandResult());
        }
    }
}