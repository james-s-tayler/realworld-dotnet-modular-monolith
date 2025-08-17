using System.Diagnostics.CodeAnalysis;
using App.Core.DataAccess;
using App.Core.PipelineBehaviors.OperationResponse;
using MediatR;

namespace App.Feed.Domain.Contracts.Operations.Queries.GetFeed
{
    [ExcludeFromCodeCoverage]
    public class GetFeedQuery : ContractModel, IRequest<OperationResponse<GetFeedQueryResult>>
    {
        public int Limit { get; set; } = 20;
        public int Offset { get; set; }
    }
}