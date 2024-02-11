
namespace MetricsConfiguration.Domain.Interface.Cache
{
    public interface ICacheMethods
    {
        Task<T> GetAsync<T>(string key);
        Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> function, DateTimeOffset? timeExpiration = null);
        void Set<T>(string key, T data, DateTimeOffset? timeExpiration = null);
        void Remove(string key);
    }
}
