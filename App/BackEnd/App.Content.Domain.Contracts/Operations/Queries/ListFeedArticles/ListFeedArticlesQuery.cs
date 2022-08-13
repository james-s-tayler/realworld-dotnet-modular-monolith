using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using App.Core.DataAccess;
using App.Core.PipelineBehaviors.OperationResponse;
using MediatR;

namespace App.Content.Domain.Contracts.Operations.Queries.ListFeedArticles
{
    [ExcludeFromCodeCoverage]
    public class ListFeedArticlesQuery : ContractModel, IRequest<OperationResponse<ListFeedArticlesQueryResult>>
    {
        [Range(1, 100)]
        public int Limit { get; set; } = 20;

        [Range(0, Int32.MaxValue)]
        public int Offset { get; set; } = 0;
    }
}