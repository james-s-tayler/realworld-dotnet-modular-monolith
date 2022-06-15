using System;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Polly;

namespace Application.Core.Caching
{
    public class Cache : ICache
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IConfiguration _configuration;
        private readonly ILogger<Cache> _logger;

        public Cache(IDistributedCache distributedCache,
            IConfiguration configuration, 
            ILogger<Cache> logger)
        {
            _distributedCache = distributedCache;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<T> GetOrRefresh<T>(string key, Func<Task<T>> refresh)
        {
            T cachedValue;
            try
            {
                cachedValue = await Get<T>(key);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Cache Error -> getting - key: {0} error: {1}", key, e.Message);
                //fallback to the provided Func and return something sensible even if it's slower
                return await refresh.Invoke();
            }
            
            if (cachedValue == null)
            {
                var cachedObject = await refresh.Invoke();
                
                _logger.LogInformation("Cache Miss -> updating - key: {CacheKey}, serializedObject: {@SerializedOjbect}", key, cachedObject);

                await Set(key, cachedObject);
                return cachedObject;
            }
            
            _logger.LogInformation("Cache Hit -> retrieved - key: {CacheKey}, deserializedObject: {@DeserializedObject}", key, cachedValue);
            return cachedValue;
        }

        public async Task<T> Get<T>(string key)
        {
            var maxRetryAttempts = _configuration.GetValue<int>("Cache:MaxRetryAttempts");
            var retry = Policy<byte[]>
                .Handle<SocketException>()
                .Or<TimeoutException>()
                .WaitAndRetryAsync(maxRetryAttempts, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
              
            var cachedValue = await retry.ExecuteAsync(async () => await _distributedCache.GetAsync(key));

            if (cachedValue == null)
                return default;
            
            var jsonToDeserialize = System.Text.Encoding.UTF8.GetString((byte[]) cachedValue);
            var cachedResult = JsonSerializer.Deserialize<T>(jsonToDeserialize);

            return cachedResult;
        }

        public async Task Set<T>(string key, T value)
        {
            byte[] objectToCache = JsonSerializer.SerializeToUtf8Bytes(value);

            var expirySeconds = _configuration.GetValue<int?>($"Cache:ExpirySeconds:ByType:{value.GetType().Name}") 
                                 ?? _configuration.GetValue<int?>("Cache:ExpirySeconds:Default")
                                 ?? throw new ArgumentNullException("Cache:ExpirySeconds");

            var cacheEntryOptions = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(expirySeconds));
            
            await _distributedCache.SetAsync(key, objectToCache, cacheEntryOptions);
        }
    }
}