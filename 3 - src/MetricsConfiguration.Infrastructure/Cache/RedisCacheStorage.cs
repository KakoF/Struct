using MetricsConfiguration.Domain.Interface.Cache;
using MetricsConfiguration.Infrastructure.Environment;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MetricsConfiguration.Infrastructure.Cache
{
    public class RedisCacheStorage : IRedisCacheStorage
    {
        private readonly IDistributedCache _redisCache;
        private readonly DistributedCacheEntryOptions _redisOptions;
        private readonly ILogger<RedisCacheStorage> _logger;
        private readonly string _cache_key;

        public RedisCacheStorage(IDistributedCache redisCache, CacheConfigs configs, ILogger<RedisCacheStorage> logger)
        {
            _redisCache = redisCache;
            _cache_key = configs.RedisCache.Key;
            _redisOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(configs.RedisCache.Expriration_Minutes),
            };
            _logger = logger;
        }

        public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> function, DateTimeOffset? timeExpiration = null)
        {
            var resultRedis = await GetAsync<T>(key);
            if (resultRedis != null) return resultRedis;

            var data = await function();

            Set(key, data, timeExpiration);

            return data;
        }

        public void Set<T>(string key, T data, DateTimeOffset? timeExpiration = null)
        {
            try
            {
                if (timeExpiration != null)
                    _redisOptions.AbsoluteExpiration = timeExpiration;
                _redisCache.Set($"{_cache_key}:{key}", System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data)), _redisOptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Não foi possível setar o cache do Redis, chave -> {_cache_key}:{key}");
            }
        }

        public async Task<T> GetAsync<T>(string key)
        {
            try
            {
                var json = await _redisCache.GetStringAsync($"{_cache_key}:{key}");
                return (json == null) ? default(T) : JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Não foi possível recuperar o cache do Redis, chave -> {_cache_key}:{key}");
                return default(T);
            }
        }

        public void Remove(string key)
        {
            try
            {
                _redisCache.Remove($"{_cache_key}:{key}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Não foi possível limpar o cache do Redis, chave -> {_cache_key}:{key}");
            }

        }


    }
}