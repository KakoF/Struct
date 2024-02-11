using MetricsConfiguration.Domain.Interface.Client;
using MetricsConfiguration.Domain.Interface.Repositories;
using MetricsConfiguration.Domain.Interface.Services;
using MetricsConfiguration.Domain.Model.Client;

namespace MetricsConfiguration.Service.Services
{
    public class AdviceService : IAdviceService
    {
        private readonly IAdviceClient _client;
        private readonly IOriginRepository _originRepository;
        public AdviceService(IAdviceClient client, IOriginRepository originRepository)
        {
            _client = client;
            _originRepository = originRepository;
        }
        public async Task<AdviceModel> GetRandomAdviceAsync()
        {
            var model = await _originRepository.GetAsync();
            return await _client.GetAsync<AdviceModel>("advice");
        }
    }
}
