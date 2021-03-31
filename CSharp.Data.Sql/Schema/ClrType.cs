namespace CSharp.Data.Sql.Schema
{
    using System;

    public sealed record ClrType
    {
        public ClrType(Type columnType) => TypeName = columnType.FullName;

        public string TypeName { get; }
    }
}
