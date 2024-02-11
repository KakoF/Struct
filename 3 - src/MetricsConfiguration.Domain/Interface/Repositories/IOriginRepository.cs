using MetricsConfiguration.Domain.Model.Origin;
namespace MetricsConfiguration.Domain.Interface.Repositories
{
    public interface IOriginRepository
    {
        Task<IEnumerable<OriginModel>> GetAsync();
    }
}
