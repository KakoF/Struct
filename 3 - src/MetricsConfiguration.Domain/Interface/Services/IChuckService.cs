using MetricsConfiguration.Domain.Model.Client;
namespace MetricsConfiguration.Domain.Interface.Services
{
    public interface IChuckService
    {
        Task<ChuckNorrisModel> GetRandomJokeAsync();
    }
}
