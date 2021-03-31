namespace CSharp.Data.Sql.Common
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Schema;
    using Util;

    public class SqlDataProvider {}

    public static class SqlDataProviderUtilities
    {
        public static string GetSqlDataProviderClassSyntax() => @$"
    // Generated 
    public class {nameof(SqlDataProvider)} {{}}";

        public static string GetPartialSqlDataProviderClassSyntax(string className, string nameSpace, IEnumerable<Table> tables)
        {
            var classTextStart = $@"
using CSharp.Data.Sql;

namespace {nameSpace}
{{
    // Generated
    public partial class {className} : {nameof(SqlDataProvider)}
    {{
        public {className}(string connectionString) =>
            DataContext = new System.Data.Linq.DataContext(connectionString);

        public System.Data.Linq.DataContext DataContext {{ get; }}";

            var builder = new StringBuilder(classTextStart);

            builder.AppendCollection(tables.Select(GetQueryablePropertyFromTable));

            var classTextEnd = @$"
    }}
}}";

            builder.Append(classTextEnd);
            return builder.ToString();

            static string GetQueryablePropertyFromTable(Table table) => @$"
        public System.Linq.IQueryable<{table.TableName}> {table.TableName} {{ get => DataContext.GetTable<{table.TableName}>(); }}
";

        }
    }
}