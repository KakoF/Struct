using Dapper;
using MetricsConfiguration.Domain.Interface.Repositories;
using MetricsConfiguration.Domain.Model.Origin;
using MetricsConfiguration.Infrastructure.Interfaces.DataConnector;

namespace MetricsConfiguration.Infrastructure.Repositories
{
    public class OriginRepository : IOriginRepository
    {
        private readonly IDbPostgreConnector _connector;
        public OriginRepository(IDbPostgreConnector connector)
        {
            _connector = connector;
        }
        public async Task<IEnumerable<OriginModel>> GetAsync()
        {
            return await _connector.dbConnection.QueryAsync<OriginModel>("Select Id, Name from Origins.Origins");

        }
    }
}
