using System.Threading;
using System.Threading.Tasks;
using App.Content.Domain.Contracts.Operations.Commands.PublishArticle;
using App.Feed.Domain.Entities;
using App.Feed.Domain.Infrastructure.Repositories;
using JetBrains.Annotations;
using MediatR;

namespace App.Feed.Domain.Infrastructure.EventListeners
{
    internal class PublishArticleCommandResultListener : INotificationHandler<PublishArticleCommandResult>
    {
        private readonly IArticleRepository _articleRepository;

        public PublishArticleCommandResultListener([NotNull] IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public async Task Handle(PublishArticleCommandResult publishArticleEvent, CancellationToken cancellationToken)
        {
            var publishedArticle = new ArticleEntity
            {
                ArticleId = publishArticleEvent.ArticleId,
                UserId = publishArticleEvent.UserId,
                CreatedAt = publishArticleEvent.Article.CreatedAt
            };
            _ = await _articleRepository.Insert(publishedArticle);
        }
    }
}