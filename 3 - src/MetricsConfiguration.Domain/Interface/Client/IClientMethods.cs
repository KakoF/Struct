namespace MetricsConfiguration.Domain.Interface.Client
{
    public interface IClientMethods
    {
        Task<T> GetAsync<T>(string path);
        Task<T> PostAsync<T>(string path, object data);
        Task<T> PutAsync<T>(string path, object data);
        Task<T> DeleteAsync<T>(string path);
    }
}
