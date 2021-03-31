namespace CSharp.Data.Sql.Schema.Provider.SqlServer
{
    using System.Collections.Generic;
    using System.Data;

    public interface ISchemaDataAdapter
    {
        IEnumerable<string> GetTableList();
        DataTable GetSchemaDataTable(string tableName);
    }
}