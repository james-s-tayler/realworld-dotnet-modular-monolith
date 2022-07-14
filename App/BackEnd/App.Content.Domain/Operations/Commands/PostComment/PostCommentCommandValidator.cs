using System.Threading;
using System.Threading.Tasks;
using App.Content.Domain.Contracts.Operations.Commands.PostComment;
using App.Content.Domain.Infrastructure.Repositories;
using FluentValidation;
using JetBrains.Annotations;

namespace App.Content.Domain.Operations.Commands.PostComment
{
    internal class PostCommentCommandValidator : AbstractValidator<PostCommentCommand>
    {
        private readonly IArticleRepository _articleRepository;

        public PostCommentCommandValidator([NotNull] IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
            
            RuleFor(command => command)
                .MustAsync(SlugMustExist)
                .WithMessage(command => $"Article with slug: {command.ArticleSlug} does not exist");
        }

        private async Task<bool> SlugMustExist(PostCommentCommand command, CancellationToken cancellationToken)
        {
            return await _articleRepository.ExistsBySlug(command.ArticleSlug);
        }
    }
}