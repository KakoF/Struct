using MetricsConfiguration.Infrastructure.Interfaces.DataConnector;
using Microsoft.Data.SqlClient;
using System.Data;

namespace MetricsConfiguration.Infrastructure.DataConnector.Abstract
{
    public class DbConnectorAbstract : IDbConnector
    {
        public DbConnectorAbstract(string connectionString, IDbConnection db)
        {
            dbConnection = db;
            dbConnection.ConnectionString = connectionString;
            dbConnection.Open();
        }

        public IDbConnection dbConnection { get; }

        public IDbTransaction dbTransaction { get; set; }

        public IDbTransaction BeginTransaction(IsolationLevel isolation)
        {
            if (dbTransaction != null)
            {
                return dbTransaction;
            }

            if (dbConnection.State == ConnectionState.Closed)
            {
                dbConnection.Open();
            }

            return (dbTransaction = dbConnection.BeginTransaction(isolation));
        }

        public void Dispose()
        {
            dbTransaction?.Dispose();
            dbConnection?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
