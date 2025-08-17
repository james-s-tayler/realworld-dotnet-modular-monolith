using System;
using App.Feed.Domain.Contracts.Operations.Queries.GetFeed;
using FluentValidation;

namespace App.Feed.Domain.Operations.Queries.GetFeed;

internal class GetFeedQueryValidator : AbstractValidator<GetFeedQuery>
{
    public GetFeedQueryValidator()
    {
        RuleFor(query => query.Limit).InclusiveBetween(1, 100);
        RuleFor(query => query.Offset).InclusiveBetween(0, Int32.MaxValue);
    }
}