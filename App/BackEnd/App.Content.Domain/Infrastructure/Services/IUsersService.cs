using System.Threading.Tasks;
using App.Core.PipelineBehaviors.OperationResponse;
using App.Users.Domain.Contracts.Operations.Queries.GetProfile;
using JetBrains.Annotations;

namespace App.Content.Domain.Infrastructure.Services
{
    internal interface IUsersService
    {
        Task<OperationResponse<GetProfileQueryResult>> GetProfile([NotNull] string username);
    }
}