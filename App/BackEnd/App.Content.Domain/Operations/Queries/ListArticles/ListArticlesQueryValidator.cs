using App.Content.Domain.Contracts.Operations.Queries.ListArticles;
using FluentValidation;

namespace App.Content.Domain.Operations.Queries.ListArticles
{
    internal class ListArticlesQueryValidator : AbstractValidator<ListArticlesQuery>
    {
        public ListArticlesQueryValidator()
        {
            // empty query params should return 422
            // http://conduit-api/api/articles?tag= 
            // http://conduit-api/api/articles?author=
            // http://conduit-api/api/articles?favorited= 
           // RuleFor(query => query.Offset).GreaterThanOrEqualTo(0);
        }
    }
}