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
                .MustAsync(ContainUpdate)
                .WithMessage(_ => "Must contain an update");
        }

        private async Task<bool> ContainUpdate(EditArticleCommand command, CancellationToken cancellationToken)
        {
            return command.UpdatedArticle.Title != null ||
                   command.UpdatedArticle.Description != null ||
                   command.UpdatedArticle.Body != null;
        }
    }
}