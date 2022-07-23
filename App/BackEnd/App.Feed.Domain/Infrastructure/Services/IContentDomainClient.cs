using System.Threading.Tasks;
using App.Content.Domain.Contracts.Operations.Queries.GetArticleById;
using App.Core.PipelineBehaviors.OperationResponse;

namespace App.Feed.Domain.Infrastructure.Services
{
    public interface IContentDomainClient
    {
        Task<OperationResponse<GetArticleByIdQueryResult>> GetArticleById(GetArticleByIdQuery query);
    }
}