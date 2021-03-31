namespace CSharp.Data.Sql.Schema
{
    using System;

    public sealed record Column(string ColumnName, ClrType ClrType, bool IsPrimaryKey, bool AllowNull)
    {
        public string ColumnName { get; init; } = 
            ColumnName ?? throw new ArgumentNullException($"Column Name must not be null {nameof(ColumnName)}");

        public ClrType ClrType { get; init; } =
            ClrType ?? throw new ArgumentNullException($"ClrType must not be null {nameof(ClrType)}");
    }

    public static class ColumnUtilities
    {
        public static string ProvidePropertyTextFromColumn(Column column) =>
            $@"
        [System.Data.Linq.Mapping.Column{(column.IsPrimaryKey ? "(IsPrimaryKey = true)" : "")}]
        public {column.ClrType.TypeName}{(column.AllowNull ? "?" : "")} {column.ColumnName} {{ get; set; }}
";
    }
}

//public {column.ClrType.TypeName}{(column.AllowNull ? "?" : "")} {column.ColumnName} {{ get; set; }}
