using MetricsConfiguration.Infrastructure.DataConnector.Abstract;
using MetricsConfiguration.Infrastructure.Interfaces.DataConnector;
using Microsoft.Data.SqlClient;

namespace MetricsConfiguration.Infrastructure.DataConnector
{
    public class SqlServerConnector : DbConnectorAbstract, IDbSqlServerConnector
    {
        public SqlServerConnector(string connectionString) : base(connectionString, SqlClientFactory.Instance.CreateConnection())
        {
        }
    }
}
