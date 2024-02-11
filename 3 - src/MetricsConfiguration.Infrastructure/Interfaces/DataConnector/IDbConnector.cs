using System.Data;
namespace MetricsConfiguration.Infrastructure.Interfaces.DataConnector
{
    public interface IDbConnector : IDisposable
    {
        IDbConnection dbConnection { get; }
        IDbTransaction dbTransaction { get; set; }
        IDbTransaction BeginTransaction(IsolationLevel isolation);
    }
}
