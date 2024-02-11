using MetricsConfiguration.Domain.Interface.Client;
using MetricsConfiguration.Domain.Interface.Services;
using MetricsConfiguration.Domain.Model.Client;

namespace MetricsConfiguration.Service.Services
{
    public class AdviceService : IAdviceService
    {
        private readonly IAdviceClient _client;
        public AdviceService(IAdviceClient client)
        {
            _client = client;
        }
        public async Task<AdviceModel> GetRandomAdviceAsync()
        {
            return await _client.GetAsync<AdviceModel>("advice");
        }
    }
}
