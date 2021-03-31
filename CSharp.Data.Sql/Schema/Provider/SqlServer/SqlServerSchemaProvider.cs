namespace CSharp.Data.Sql.Schema.Provider.SqlServer
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using Generator.Errors;
    using Util.Func;

    public static class SqlServerSchemaProvider
    {
        //TODO unit test this.
        public static async Task<Result<IReadOnlyCollection<Table>>> GetTablesFromConnectionStringAsync(IDbConnectionAsync connection, Func<ISchemaDataAdapter> adapterFactory )
        {
            try
            {
                await connection.OpenAsync();
                var adapter = adapterFactory();

                var tablesNames = adapter.GetTableList();

                var result = tablesNames
                    .Select(x => GetTable(adapter, x))
                    .ToList();

                return Success<IReadOnlyCollection<Table>>.Succeed(result);
            }
            catch (Exception e)
            {
                //var d = Failure<SqlError>.Fail(new SqlError());
                //return (Result<IReadOnlyCollection<Table>>)d;
                return Failure<IReadOnlyCollection<Table>, SqlError>.Fail(new SqlError {Message = e.Message});
            }
        }

        public static Table GetTable(ISchemaDataAdapter adapter, string tableName)
        {
            using var table = adapter.GetSchemaDataTable(tableName);

            var nonPrimaryKeyColumns = table
                .Columns.Cast<DataColumn>().Except(table.PrimaryKey).Select(x => GetColumnFromDataColumn(x, false));

            var primaryKeyColumns = table
                .PrimaryKey.Select(x => GetColumnFromDataColumn(x, true));

            return new(tableName, primaryKeyColumns.Union(nonPrimaryKeyColumns).ToArray());

            static Column GetColumnFromDataColumn(DataColumn dataColumn, bool isPrimaryKey) =>
                new (dataColumn.ColumnName, new(dataColumn.DataType), isPrimaryKey, dataColumn.AllowDBNull);
        }
    }
}