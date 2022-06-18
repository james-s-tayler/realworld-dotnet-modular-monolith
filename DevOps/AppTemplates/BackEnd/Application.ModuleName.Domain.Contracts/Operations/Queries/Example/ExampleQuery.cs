using Application.Core.DataAccess;
using Application.Core.PipelineBehaviors.OperationResponse;
using MediatR;

namespace Application.ModuleName.Domain.Contracts.Operations.Queries.Example
{
    public class ExampleQuery : ContractModel, IRequest<OperationResponse<ExampleQueryResult>> {}
}