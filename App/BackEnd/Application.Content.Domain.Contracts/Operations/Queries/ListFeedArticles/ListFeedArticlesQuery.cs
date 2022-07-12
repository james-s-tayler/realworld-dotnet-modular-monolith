using System;
using System.ComponentModel.DataAnnotations;
using Application.Core.DataAccess;
using Application.Core.PipelineBehaviors.OperationResponse;
using MediatR;

namespace Application.Content.Domain.Contracts.Operations.Queries.ListFeedArticles
{
    public class ListFeedArticlesQuery : ContractModel, IRequest<OperationResponse<ListFeedArticlesQueryResult>>
    {
        [Range(1, 100)]
        public int Limit { get; set; } = 20;
        
        [Range(0, Int32.MaxValue)]
        public int Offset { get; set; } = 0;
    }
}