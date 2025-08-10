using System.Threading;
using System.Threading.Tasks;
using App.Content.Domain.Contracts.Operations.Commands.DeleteArticle;
using App.Feed.Domain.Entities;
using App.Feed.Domain.Infrastructure.Repositories;
using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Logging;

namespace App.Feed.Domain.Infrastructure.EventListeners
{
    internal class DeleteArticleCommandResultListener : INotificationHandler<DeleteArticleCommandResult>
    {
        private readonly IArticleRepository _articleRepository;

        public DeleteArticleCommandResultListener([NotNull] IArticleRepository articleRepository, ILogger<DeleteArticleCommandResultListener> logger)
        {
            _articleRepository = articleRepository;
        }

        public async Task Handle(DeleteArticleCommandResult deleteArticleEvent, CancellationToken cancellationToken)
        {
            await _articleRepository.Delete(new ArticleEntity
            {
                ArticleId = deleteArticleEvent.ArticleId,
                UserId = deleteArticleEvent.UserId
            });
        }
    }
}