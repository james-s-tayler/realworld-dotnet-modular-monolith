using System;
using App.Content.Domain.Contracts.Operations.Queries.ListArticles;
using FluentValidation;

namespace App.Content.Domain.Operations.Queries.ListArticles;

internal class ListArticlesQueryValidator :  AbstractValidator<ListArticlesQuery>
{
    public ListArticlesQueryValidator()
    {
        RuleFor(query => query.Limit).InclusiveBetween(1, 100);
        RuleFor(query => query.Offset).InclusiveBetween(0, Int32.MaxValue);
    }
}