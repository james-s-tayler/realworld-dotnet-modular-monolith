using System.Diagnostics.CodeAnalysis;
using App.Core.DataAccess;
using App.Core.PipelineBehaviors.Authorization;
using App.Core.PipelineBehaviors.OperationResponse;
using MediatR;

namespace App.Users.Domain.Contracts.Operations.Queries.GetProfile
{
    [ExcludeFromCodeCoverage]
    [AllowUnauthenticated]
    public class GetProfileQuery : ContractModel, IRequest<OperationResponse<GetProfileQueryResult>>
    {
        public string Username { get; set; }
    }
}