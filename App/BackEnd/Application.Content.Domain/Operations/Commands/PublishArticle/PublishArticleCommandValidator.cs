using System.Threading;
using System.Threading.Tasks;
using Application.Content.Domain.Contracts.Operations.Commands.PublishArticle;
using Application.Content.Domain.Infrastructure.Repositories;
using FluentValidation;
using JetBrains.Annotations;

namespace Application.Content.Domain.Operations.Commands.PublishArticle
{
    internal class PublishArticleCommandValidator : AbstractValidator<PublishArticleCommand>
    {
        private readonly IArticleRepository _articleRepository;

        public PublishArticleCommandValidator([NotNull] IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
            
            RuleFor(command => command)
                .MustAsync(SlugMustNotExist)
                .WithMessage(command => $"Article with slug: {command.NewArticle.GetSlug()} already exists");
        }

        private async Task<bool> SlugMustNotExist(PublishArticleCommand command, CancellationToken cancellationToken)
        {
            var exists = await _articleRepository.ExistsBySlug(command.NewArticle.GetSlug());
            
            return !exists;
        }
    }
}