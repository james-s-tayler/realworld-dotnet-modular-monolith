using System;
using System.ComponentModel.DataAnnotations;
using Application.Core.DataAccess;
using Application.Core.PipelineBehaviors.Authorization;
using Application.Core.PipelineBehaviors.OperationResponse;
using MediatR;

namespace Application.Content.Domain.Contracts.Operations.Queries.ListArticles
{
    [AllowUnauthenticated]
    public class ListArticlesQuery : ContractModel, IRequest<OperationResponse<ListArticlesQueryResult>>
    {
        public string Tag { get; set; }
        public string AuthorUsername { get; set; }
        public string FavoritedByUsername { get; set; }
        
        [Range(1, 100)]
        public int Limit { get; set; } = 20;
        
        [Range(0, Int32.MaxValue)]
        public int Offset { get; set; } = 0;
    }
}