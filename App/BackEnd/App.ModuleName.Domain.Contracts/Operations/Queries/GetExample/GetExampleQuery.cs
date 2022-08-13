using System.Diagnostics.CodeAnalysis;
using App.Core.DataAccess;
using App.Core.PipelineBehaviors.OperationResponse;
using MediatR;

namespace App.ModuleName.Domain.Contracts.Operations.Queries.GetExample
{
    [ExcludeFromCodeCoverage]
    public class GetExampleQuery : ContractModel, IRequest<OperationResponse<GetExampleQueryResult>>
    {
        public int Id { get; set; }
    }
}