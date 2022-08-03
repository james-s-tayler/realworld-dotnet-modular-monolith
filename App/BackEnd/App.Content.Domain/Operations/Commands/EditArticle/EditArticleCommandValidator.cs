using System.Threading;
using System.Threading.Tasks;
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
                .WithMessage(_ => "Must contain an update");
        }

        private bool ContainUpdate(EditArticleCommand command)
        {
            return command.UpdatedArticle.Title != null ||
                   command.UpdatedArticle.Description != null ||
                   command.UpdatedArticle.Body != null;
        }
    }
}