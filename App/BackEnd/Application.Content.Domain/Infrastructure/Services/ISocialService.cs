using System.Threading.Tasks;
using Application.Core.PipelineBehaviors.OperationResponse;
using Application.Social.Domain.Contracts.Operations.Queries.GetProfile;
using JetBrains.Annotations;

namespace Application.Content.Domain.Infrastructure.Services
{
    internal interface ISocialService
    {
        Task<OperationResponse<GetProfileQueryResult>> GetProfile([NotNull] string username);
    }
}