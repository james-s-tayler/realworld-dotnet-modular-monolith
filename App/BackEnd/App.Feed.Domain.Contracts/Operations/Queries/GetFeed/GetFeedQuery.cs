using System.ComponentModel.DataAnnotations;
using App.Core.DataAccess;
using App.Core.PipelineBehaviors.OperationResponse;
using App.Feed.Domain.Contracts.DTOs;
using MediatR;

namespace App.Feed.Domain.Contracts.Operations.Queries.GetFeed
{
    //[AllowUnauthenticated] - for unauthorized requests
    public class GetFeedQuery : ContractModel, IRequest<OperationResponse<GetFeedQueryResult>>
    {
        [Required]
        public ExampleDTO ExampleInput { get; set; }
    } 
}