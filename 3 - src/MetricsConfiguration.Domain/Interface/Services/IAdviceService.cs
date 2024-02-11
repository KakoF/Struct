using MetricsConfiguration.Domain.Model.Client;
namespace MetricsConfiguration.Domain.Interface.Services
{
    public interface IAdviceService
    {
        Task<AdviceModel> GetRandomAdviceAsync();
    }
}
