using System.Threading;
using System.Threading.Tasks;
using Application.Content.Domain.Contracts.Operations.Queries.GetSingleArticle;
using Application.Content.Domain.Infrastructure.Repositories;
using FluentValidation;
using JetBrains.Annotations;

namespace Application.Content.Domain.Operations.Queries.GetSingleArticle
{
    internal class GetSingleArticleQueryValidator : AbstractValidator<GetSingleArticleQuery>
    {
        private readonly IArticleRepository _articleRepository;

        public GetSingleArticleQueryValidator([NotNull] IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;

            RuleFor(query => query).MustAsync(ArticleMustExist)
                .WithMessage(query => $"Example {query.Slug} was not found.");
        }

        private async Task<bool> ArticleMustExist(GetSingleArticleQuery query, CancellationToken cancellationToken)
        {
            return await _articleRepository.ExistsBySlug(query.Slug);
        }
    }
}