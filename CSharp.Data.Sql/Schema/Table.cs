namespace CSharp.Data.Sql.Schema
{
    using System;
    using System.Linq;
    using System.Text;
    using Util;

    using static ColumnUtilities;

    public sealed record Table(string TableName, Column[] Columns)
    {
        public string TableName { get; init; } = TableName ?? throw new ArgumentNullException($"Table name must not be null {nameof(TableName)}");
    }

    public static class TableUtilities
    {
        public static string ProvideDataClassTextFromTable(Table table)
        {
            var tableStart = $@"
    [System.Data.Linq.Mapping.Table(Name = ""{table.TableName}"")]
    public class {table.TableName}
    {{";
            var builder = new StringBuilder(tableStart);

            var tableHasColumns = table.Columns.Any();

            if (tableHasColumns)
                builder.AppendCollection(table.Columns.Select(ProvidePropertyTextFromColumn));

            var tableEnd = $@"
    }}";
            builder.Append(tableEnd);
            return builder.ToString();
        }
    }
}