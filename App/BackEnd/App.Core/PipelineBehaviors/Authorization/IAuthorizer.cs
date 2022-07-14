using System.Threading;
using System.Threading.Tasks;

namespace App.Core.PipelineBehaviors.Authorization
{
    public interface IAuthorizer<T>
    {
        Task<AuthorizationResult> AuthorizeAsync(T instance, CancellationToken cancellation = default);
    }
}