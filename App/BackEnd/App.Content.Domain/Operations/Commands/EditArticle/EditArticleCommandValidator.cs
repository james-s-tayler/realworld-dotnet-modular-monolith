using App.Content.Domain.Contracts.Operations.Commands.EditArticle;
using FluentValidation;

namespace App.Content.Domain.Operations.Commands.EditArticle
{
    internal class EditArticleCommandValidator : AbstractValidator<EditArticleCommand>
    {
        public EditArticleCommandValidator()
        {
            RuleFor(command => command)
                .Must(ContainUpdate)
                .WithName("article")
                .WithMessage(_ => "must contain an update");
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
    }
}