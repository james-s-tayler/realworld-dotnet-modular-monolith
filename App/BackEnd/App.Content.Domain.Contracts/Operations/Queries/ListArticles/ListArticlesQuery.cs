using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using App.Core.DataAccess;
using App.Core.PipelineBehaviors.Authorization;
using App.Core.PipelineBehaviors.OperationResponse;
using MediatR;

namespace App.Content.Domain.Contracts.Operations.Queries.ListArticles
{
    [ExcludeFromCodeCoverage]
    [AllowUnauthenticated]
    public class ListArticlesQuery : ContractModel, IRequest<OperationResponse<ListArticlesQueryResult>>
    {
        public string Tag { get; set; }
        public string AuthorUsername { get; set; }
        public string FavoritedByUsername { get; set; }
        public int Limit { get; set; } = 20;
        public int Offset { get; set; } = 0;
    }
}