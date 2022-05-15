using System;
using System.Threading.Tasks;

namespace Conduit.Core.Caching
{
    public interface ICache
    {
        Task<T> GetOrRefresh<T>(string key, Func<Task<T>> refresh);
        Task<T> Get<T>(string key);
        Task Set<T>(string key, T value);
    }
}