using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using App.Core.DataAccess;
using App.Core.PipelineBehaviors.OperationResponse;
using MediatR;

namespace App.Feed.Domain.Contracts.Operations.Queries.GetFeed
{
    [ExcludeFromCodeCoverage]
    public class GetFeedQuery : ContractModel, IRequest<OperationResponse<GetFeedQueryResult>>
    {
        [Range(1, 100)]
        public int Limit { get; set; } = 20;
        [Range(0, Int32.MaxValue)]
        public int Offset { get; set; }
    }
}