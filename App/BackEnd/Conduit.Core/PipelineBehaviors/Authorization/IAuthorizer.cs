using System.Threading;
using System.Threading.Tasks;

namespace Conduit.Core.PipelineBehaviors.Authorization
{
    public interface IAuthorizer<T>
    {
        Task<AuthorizationResult> AuthorizeAsync(T instance, CancellationToken cancellation = default);
    }
}