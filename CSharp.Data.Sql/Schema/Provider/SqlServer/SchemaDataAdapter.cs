namespace CSharp.Data.Sql.Schema.Provider.SqlServer
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;

    public class SchemaDataAdapter : ISchemaDataAdapter
    {
        private readonly IDbConnectionAsync _connection;

        public SchemaDataAdapter(IDbConnectionAsync connection) =>
            _connection = connection;

        public IEnumerable<string> GetTableList()
        {
            const string sql = "Select DISTINCT(name) FROM sys.Tables";
            using var command = new SqlCommand(sql, (SqlConnection)_connection.GetDbConnection());
            using var adapter = new SqlDataAdapter {SelectCommand = command};

            var dataSet = new DataSet();

            adapter.Fill(dataSet);

            const string columnSelector = "name";
            return dataSet.Tables[0].Rows.Cast<DataRow>().Select(x => (string)x[columnSelector]);
        }

        public DataTable GetSchemaDataTable(string tableName)
        {
            var sql = $"select * from {tableName}";
            var command = new SqlCommand(sql, (SqlConnection)_connection.GetDbConnection());
            var adapter = new SqlDataAdapter { SelectCommand = command };

            using var table = new DataTable(tableName);

            adapter.FillSchema(table, SchemaType.Mapped);

            return table;
        }
    }
}