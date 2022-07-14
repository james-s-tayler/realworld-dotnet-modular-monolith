using System.Threading;
using System.Threading.Tasks;
using App.Content.Domain.Contracts.Operations.Commands.DeleteComment;
using App.Content.Domain.Infrastructure.Repositories;
using FluentValidation;
using JetBrains.Annotations;

namespace App.Content.Domain.Operations.Commands.DeleteComment
{
    internal class DeleteCommentCommandValidator : AbstractValidator<DeleteCommentCommand>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IArticleRepository _articleRepository;

        public DeleteCommentCommandValidator([NotNull] IArticleRepository articleRepository, ICommentRepository commentRepository)
        {
            _articleRepository = articleRepository;
            _commentRepository = commentRepository;

            RuleFor(command => command)
                .MustAsync(SlugMustExist)
                .WithMessage(command => $"Article with slug: {command.ArticleSlug} does not exist");
            RuleFor(command => command)
                .MustAsync(CommentMustBelongToArticle)
                .WithMessage(command => $"Comment: {command.CommentId} does not belong to Article: {command.ArticleSlug}");
        }

        private async Task<bool> SlugMustExist(DeleteCommentCommand command, CancellationToken cancellationToken)
        {
            return await _articleRepository.ExistsBySlug(command.ArticleSlug);
        }
        
        private async Task<bool> CommentMustBelongToArticle(DeleteCommentCommand command, CancellationToken cancellationToken)
        {
            return await _commentRepository.ExistsBySlugAndId(command.ArticleSlug, command.CommentId);
        }
    }
}