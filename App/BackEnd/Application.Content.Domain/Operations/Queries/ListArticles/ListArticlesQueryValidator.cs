using Application.Content.Domain.Contracts.Operations.Queries.ListArticles;
using FluentValidation;

namespace Application.Content.Domain.Operations.Queries.ListArticles
{
    internal class ListArticlesQueryValidator : AbstractValidator<ListArticlesQuery>
    {
        public ListArticlesQueryValidator()
        {
            //RuleFor(query => query.Offset).GreaterThanOrEqualTo(0);
        }
    }
}