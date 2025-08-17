using System.Threading;
using System.Threading.Tasks;
using App.Content.Domain.Contracts.Operations.Commands.EditArticle;
using App.Content.Domain.Infrastructure.Repositories;
using FluentValidation;

namespace App.Content.Domain.Operations.Commands.EditArticle
{
    internal class EditArticleCommandValidator : AbstractValidator<EditArticleCommand>
    {
        
        private readonly IArticleRepository _articleRepository;
        public EditArticleCommandValidator(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
            
            RuleFor(command => command)
                .Must(ContainUpdate)
                .WithName("article")
                .WithMessage(_ => "must contain an update");
            RuleFor(command => command)
                .MustAsync(SlugMustNotExist)
                .WithName("slug")
                .WithMessage(command => $"article with slug: {command.UpdatedArticle.GetSlug()} already exists");
            RuleFor(command => command.UpdatedArticle.Title)
                .NotEmpty()
                .When(command => command.UpdatedArticle.Title != null);
            RuleFor(command => command.UpdatedArticle.Body)
                .NotEmpty()
                .When(command => command.UpdatedArticle.Body != null);
            RuleFor(command => command.UpdatedArticle.Description)
                .NotEmpty()
                .When(command => command.UpdatedArticle.Description != null);
        }

        private bool ContainUpdate(EditArticleCommand command)
        {
            return command.UpdatedArticle.Title != null ||
                   command.UpdatedArticle.Description != null ||
                   command.UpdatedArticle.Body != null;
        }
        
        private async Task<bool> SlugMustNotExist(EditArticleCommand command, CancellationToken cancellationToken)
        {
            var exists = await _articleRepository.ExistsBySlug(command.UpdatedArticle.GetSlug());

            return !exists;
        }
    }
}