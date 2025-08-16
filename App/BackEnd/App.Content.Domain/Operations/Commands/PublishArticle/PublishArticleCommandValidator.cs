using System.Threading;
using System.Threading.Tasks;
using App.Content.Domain.Contracts.Operations.Commands.PublishArticle;
using App.Content.Domain.Infrastructure.Repositories;
using FluentValidation;
using JetBrains.Annotations;

namespace App.Content.Domain.Operations.Commands.PublishArticle
{
    internal class PublishArticleCommandValidator : AbstractValidator<PublishArticleCommand>
    {
        private readonly IArticleRepository _articleRepository;

        public PublishArticleCommandValidator([NotNull] IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;

            RuleFor(command => command.NewArticle.Title)
                .NotEmpty();
            RuleFor(command => command.NewArticle.Description)
                .NotEmpty();
            RuleFor(command => command.NewArticle.Body)
                .NotEmpty();
            RuleFor(command => command)
                .MustAsync(SlugMustNotExist)
                .WithMessage(command => $"Article with slug: {command.NewArticle.GetSlug()} already exists");
            RuleFor(command => command.NewArticle.TagList)
                .ForEach(tag => tag.NotEmpty());
            RuleFor(command => command.NewArticle.TagList)
                .ForEach(tag => tag.Must(t => !t.Contains(',')));
        }

        private async Task<bool> SlugMustNotExist(PublishArticleCommand command, CancellationToken cancellationToken)
        {
            var exists = await _articleRepository.ExistsBySlug(command.NewArticle.GetSlug());

            return !exists;
        }
    }
}