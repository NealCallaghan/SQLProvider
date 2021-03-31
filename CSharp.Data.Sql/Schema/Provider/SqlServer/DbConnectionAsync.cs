namespace CSharp.Data.Sql.Schema.Provider.SqlServer
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    public class DbConnectionAsync : IDbConnectionAsync, IDisposable
    {
        private readonly IDbConnection _dbConnection;

        public DbConnectionAsync(string connectionString) =>
            _dbConnection = new SqlConnection(connectionString);

        public async Task OpenAsync() =>
            await ((SqlConnection) _dbConnection).OpenAsync();

        public IDbConnection GetDbConnection() =>
            _dbConnection;

        public void Dispose() =>
            _dbConnection?.Dispose();
    }
}