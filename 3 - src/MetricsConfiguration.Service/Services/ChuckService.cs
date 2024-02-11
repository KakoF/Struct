using MetricsConfiguration.Domain.Interface.Client;
using MetricsConfiguration.Domain.Interface.Services;
using MetricsConfiguration.Domain.Model.Client;

namespace MetricsConfiguration.Service.Services
{
    public class ChuckService : IChuckService
    {
        private readonly IChuckClient _client;
        public ChuckService(IChuckClient client)
        {
            _client = client;
        }
        public async Task<ChuckNorrisModel> GetRandomJokeAsync()
        {
            return await _client.GetAsync<ChuckNorrisModel>("jokes/random");
        }
    }
}
