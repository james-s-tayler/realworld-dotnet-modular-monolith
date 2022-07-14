using App.Content.Domain.Contracts.Operations.Queries.ListArticles;
using FluentValidation;

namespace App.Content.Domain.Operations.Queries.ListArticles
{
    internal class ListArticlesQueryValidator : AbstractValidator<ListArticlesQuery>
    {
        public ListArticlesQueryValidator()
        {
            //RuleFor(query => query.Offset).GreaterThanOrEqualTo(0);
        }
    }
}