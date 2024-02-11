using MetricsConfiguration.Domain.Interface.Cache;
using MetricsConfiguration.Infrastructure.Environment;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace MetricsConfiguration.Infrastructure.Cache
{
    public class MemoryCacheStorage : IMemoryCacheStorage
    {
        private readonly IMemoryCache _memoryCache;
        private readonly DateTimeOffset _timeExpiration;
        private readonly ILogger<MemoryCacheStorage> _logger;
        private readonly string _cache_key;
        public MemoryCacheStorage(IMemoryCache memoryCache, CacheConfigs configs, ILogger<MemoryCacheStorage> logger)
        {
            _memoryCache = memoryCache;

            _cache_key = configs.MemoryCache.Key;
            _timeExpiration = DateTimeOffset.Now.AddMinutes(configs.MemoryCache.Expriration_Minutes);
            _logger = logger;
        }

        public Task<T> GetAsync<T>(string key)
        {
            try
            {
                return Task.FromResult(_memoryCache.Get<T>($"{_cache_key}:{key}"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Não foi possível recuperar o cache do Memory Cache, chave -> {_cache_key}:{key}");
                return Task.FromResult(default(T));
            }
        }

        public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> function, DateTimeOffset? timeExpiration = null)
        {
            var data = await _memoryCache.GetOrCreateAsync($"{_cache_key}:{key}", async entry => {
                entry.AbsoluteExpiration = timeExpiration ?? _timeExpiration;
                return await function();
            });
            return data;
        }

        public void Remove(string key)
        {
            try
            {
                _memoryCache.Remove($"{_cache_key}:{key}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Não foi possível limpar o cache do Memory Cache, chave -> {_cache_key}:{key}");
            }
        }

        public void RemoveAll()
        {
            try
            {
                _memoryCache.Remove($"{_cache_key}:*");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Não foi possível limpar o cache do Memory Cache, chave -> {_cache_key}:*");
            }
        }

        public void Set<T>(string key, T data, DateTimeOffset? timeExpiration = null)
        {
            try
            {
                _memoryCache.Set($"{_cache_key}:{key}", data, (timeExpiration ?? _timeExpiration));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Não foi possível setar o cache do Memory Cache, chave -> {_cache_key}:{key}");
            }
        }
    }
}