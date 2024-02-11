using MetricsConfiguration.Infrastructure.DataConnector.Abstract;
using MetricsConfiguration.Infrastructure.Interfaces.DataConnector;
using Npgsql;

namespace MetricsConfiguration.Infrastructure.DataConnector
{
    public class PostgreConnector : DbConnectorAbstract, IDbPostgreConnector
    {
        public PostgreConnector(string connectionString) : base(connectionString, NpgsqlFactory.Instance.CreateConnection())
        {
        }
    }
}
